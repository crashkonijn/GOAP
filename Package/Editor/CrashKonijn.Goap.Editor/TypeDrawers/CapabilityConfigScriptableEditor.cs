using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    [CustomEditor(typeof(CapabilityConfigScriptable))]
    public class CapabilityConfigScriptableEditor : UnityEditor.Editor
    {
        private CapabilityConfigScriptable scriptable;
        private GoalList goalList;
        private ActionList actionList;
        private SensorList<CapabilityWorldSensor> worldSensorList;
        private SensorList<CapabilityTargetSensor> targetSensorList;
        private SensorList<CapabilityMultiSensor> multiSensorList;

        public override VisualElement CreateInspectorGUI()
        {
            this.scriptable = (CapabilityConfigScriptable)this.target;
            
            var root = new VisualElement();
            
            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss");
            
            root.styleSheets.Add(styleSheet);
            
            
            // Create ListView
            var generator = scriptable.GetGenerator();
            
            root.Add(new Header("Goals"));
            this.goalList = new GoalList(this.serializedObject, this.scriptable, generator);
            root.Add(this.goalList);

            root.Add(new Header("Actions"));
            this.actionList = new ActionList(this.serializedObject, this.scriptable, generator);
            root.Add(this.actionList);

            root.Add(new Header("WorldSensors"));
            this.worldSensorList = new SensorList<CapabilityWorldSensor>(this.serializedObject, this.scriptable, generator, this.scriptable.worldSensors, "worldSensors");
            root.Add(this.worldSensorList);

            root.Add(new Header("TargetSensors"));
            this.targetSensorList = new SensorList<CapabilityTargetSensor>(this.serializedObject, this.scriptable, generator, this.scriptable.targetSensors, "targetSensors");
            root.Add(this.targetSensorList);

            root.Add(new Header("MultiSensors"));
            this.multiSensorList = new SensorList<CapabilityMultiSensor>(this.serializedObject, this.scriptable, generator, this.scriptable.multiSensors, "multiSensors");
            root.Add(this.multiSensorList);

            var checkButton = new Button(() =>
            {
                var issues = new ScriptReferenceValidator().CheckAll(this.scriptable);
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
                var validator = new ScriptReferenceValidator();
                
                var issues = validator.CheckAll(this.scriptable);
                
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