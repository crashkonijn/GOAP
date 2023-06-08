using System;
using System.Linq;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoapSetConfigScriptable))]
    public class GoapSetConfigEditor : UnityEditor.Editor
    {
        private GoapSetConfigScriptable config;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.config = (GoapSetConfigScriptable) this.target;
            
            var root = new VisualElement();
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss");
            root.styleSheets.Add(styleSheet);
            
            root.Add(this.Group("Debugger", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("debuggerClass")));
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
                var validator = new GoapSetConfigValidatorRunner();
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