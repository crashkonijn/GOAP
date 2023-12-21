﻿using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(CapabilityConfigScriptable))]
    public class CapabilityConfigScriptableEditor : UnityEditor.Editor
    {
        private CapabilityConfigScriptable scriptable;
        private GoalList goalList;
        private ActionList actionList;
        private SensorList<BehaviourWorldSensor> worldSensorList;
        private SensorList<BehaviourTargetSensor> targetSensorList;
        private SensorList<BehaviourMultiSensor> multiSensorList;

        public override VisualElement CreateInspectorGUI()
        {
            this.scriptable = (CapabilityConfigScriptable)this.target;
            var root = new VisualElement();
            
            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss");
            
            root.styleSheets.Add(styleSheet);
            
            
            // Create ListView
            var generator = ClassScanner.GetGenerator(this.scriptable);
            
            root.Add(new Header("Goals"));
            this.goalList = new GoalList(this.scriptable, generator);
            root.Add(this.goalList);

            root.Add(new Header("Actions"));
            this.actionList = new ActionList(this.scriptable, generator);
            root.Add(this.actionList);

            root.Add(new Header("WorldSensors"));
            this.worldSensorList = new SensorList<BehaviourWorldSensor>(this.scriptable, generator, this.scriptable.worldSensors);
            root.Add(this.worldSensorList);

            root.Add(new Header("TargetSensors"));
            this.targetSensorList = new SensorList<BehaviourTargetSensor>(this.scriptable, generator, this.scriptable.targetSensors);
            root.Add(this.targetSensorList);

            root.Add(new Header("MultiSensors"));
            this.multiSensorList = new SensorList<BehaviourMultiSensor>(this.scriptable, generator, this.scriptable.multiSensors);
            root.Add(this.multiSensorList);

            var checkButton = new Button(() =>
            {
                var issues = new ScriptReferenceValidator().Check(this.scriptable);
                if (issues.Length == 0)
                {
                    Debug.Log("No issues found!");
                    return;
                }
                
                foreach (var issue in issues)
                {
                    Debug.Log(issue.GetMessage());
                }
            });
            checkButton.Add(new Label("Check issues"));
            root.Add(checkButton);
            
            var fixButton = new Button(() =>
            {
                var issues = new ScriptReferenceValidator().Check(this.scriptable);
                
                if (issues.Length == 0)
                {
                    Debug.Log("No issues found!");
                    return;
                }
                
                foreach (var issue in issues)
                {
                    issue.Fix(this.scriptable.GetGenerator());
                }
                
                EditorUtility.SetDirty(this.scriptable);
                AssetDatabase.SaveAssetIfDirty(this.scriptable);
                AssetDatabase.Refresh();
            });
            fixButton.Add(new Label("Fix issues!"));
            root.Add(fixButton);
            
            return root;
        }
        
    }
}