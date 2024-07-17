using System;
using System.IO;
using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    [CustomEditor(typeof(GoapSetConfigScriptable))]
    [Obsolete("Use CapabilityConfigs instead!")]
    public class GoapSetConfigEditor : UnityEditor.Editor
    {
        private GoapSetConfigScriptable config;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.config = (GoapSetConfigScriptable) this.target;
            
            var root = new VisualElement();
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss");
            root.styleSheets.Add(styleSheet);
            
            root.Add(this.Group("Upgrade", card =>
            {
                // CapabilityConfigScriptable capabilityScriptable = default;
                //
                // var input = new ObjectField("CapabilityConfigs")
                // {
                //     objectType = typeof(CapabilityConfigScriptable),
                //     allowSceneObjects = false,
                //     value = capabilityScriptable
                // };
                //
                // input.RegisterValueChangedCallback(evt =>
                // {
                //     capabilityScriptable = (CapabilityConfigScriptable) evt.newValue;
                // });
                //
                // card.Add(input);
                
                card.Add(new PropertyField(this.serializedObject.FindProperty("capabilityConfig")));
                
                var button = new Button(() =>
                {
                    this.Upgrade(this.config.capabilityConfig);
                })
                {
                    text = "Upgrade to CapabilityConfigs"
                };

                card.Add(button);
            }));

            root.Add(this.Group("Goals and Actions", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("actions")));
                card.Add(new PropertyField(this.serializedObject.FindProperty("goals")));
            }));
            
            root.Add(this.Group("World Keys", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("worldSensors")));
                card.Add(this.SimpleLabelView("World keys", this.config.GetWorldKeys(), (label, key) =>
                {
                    label.text = key.Name;
                }));
            }));
            
            root.Add(this.Group("Targets", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("targetSensors")));
                card.Add(this.SimpleLabelView("Target keys", this.config.GetTargetKeys(), (label, key) =>
                {
                    label.text = key.Name;
                }));
            }));
            
            var validateButton = new Button(() =>
            {
                var validator = new AgentTypeConfigValidatorRunner();
                var results = validator.Validate(this.config);
                
                foreach (var error in results.GetErrors())
                {
                    Debug.LogError(error);
                }
            
                foreach (var warning in results.GetWarnings())
                {
                    Debug.LogWarning(warning);
                }
                
                if (!results.HasErrors() && !results.HasWarnings())
                    Debug.Log("No errors or warnings found!");
            });
            
            validateButton.Add(new Label("Validate"));

            root.Add(validateButton);

            return root;
        }

        private void Upgrade(CapabilityConfigScriptable capabilityScriptable)
        {
            capabilityScriptable.goals.Clear();
            foreach (var goalConfig in this.config.Goals)
            {
                capabilityScriptable.goals.Add(new CapabilityGoal
                {
                    goal = new ClassRef
                    {
                        Name = this.GetName(goalConfig.ClassType)
                    },
                    baseCost = goalConfig.BaseCost,
                    conditions = goalConfig.Conditions.Select(x => new CapabilityCondition
                    {
                        worldKey = new ClassRef
                        {
                            Name = x.WorldKey.Name
                        },
                        comparison = x.Comparison,
                        amount = x.Amount
                    }).ToList()
                });
            }
            
            capabilityScriptable.actions.Clear();
            foreach (var actionConfig in this.config.Actions)
            {
                capabilityScriptable.actions.Add(new CapabilityAction
                {
                    action = new ClassRef
                    {
                        Name = this.GetName(actionConfig.ClassType)
                    },
                    target = new ClassRef
                    {
                        Name = actionConfig.Target.Name
                    },
                    baseCost = actionConfig.BaseCost,
                    stoppingDistance = actionConfig.StoppingDistance,
                    conditions = actionConfig.Conditions.Select(x => new CapabilityCondition
                    {
                        worldKey = new ClassRef
                        {
                            Name = x.WorldKey.Name
                        },
                        comparison = x.Comparison,
                        amount = x.Amount
                    }).ToList(),
                    effects = actionConfig.Effects.Select(x => new CapabilityEffect
                    {
                        worldKey = new ClassRef
                        {
                            Name = x.WorldKey.Name
                        },
                        effect = x.Increase ? EffectType.Increase : EffectType.Decrease,
                    }).ToList()
                });
            }
            
            capabilityScriptable.worldSensors.Clear();
            foreach (var worldSensorConfig in this.config.WorldSensors)
            {
                capabilityScriptable.worldSensors.Add(new CapabilityWorldSensor
                {
                    sensor = new ClassRef
                    {
                        Name = this.GetName(worldSensorConfig.ClassType)
                    },
                    worldKey = new ClassRef
                    {
                        Name = worldSensorConfig.Key.Name
                    }
                });
            }
            
            capabilityScriptable.targetSensors.Clear();
            foreach (var targetSensorConfig in this.config.TargetSensors)
            {
                capabilityScriptable.targetSensors.Add(new CapabilityTargetSensor
                {
                    sensor = new ClassRef
                    {
                        Name = this.GetName(targetSensorConfig.ClassType)
                    },
                    targetKey = new ClassRef
                    {
                        Name = targetSensorConfig.Key.Name
                    }
                });
            }
            
            EditorUtility.SetDirty(capabilityScriptable);
            AssetDatabase.SaveAssetIfDirty(capabilityScriptable);
            // Set selected object to the new capability scriptable
            // Selection.activeObject = capabilityScriptable;
        }

        private string GetName(string fullName)
        {
            var parts = fullName.Split(',').First();
            
            return parts.Split('.').Last();
        }

        private VisualElement Group(string title, Action<Card> callback)
        {
            var group = new VisualElement();
            group.Add(new Header(title));
            group.Add(new Card(callback));
            return group;
        }

        private VisualElement SimpleLabelView<T>(string title, T[] list, Action<Label, T> bind)
        {
            var foldout = new Foldout()
            {
                text = title,
            };
            var listView = new ListView(list, 20, () => new Label())
            {
                bindItem = (element, index) =>
                {
                    bind(element as Label, list[index]);
                },
                selectionType = SelectionType.None
            };
            listView.AddToClassList("card");
            foldout.Add(listView);

            return foldout;
        }
    }
}