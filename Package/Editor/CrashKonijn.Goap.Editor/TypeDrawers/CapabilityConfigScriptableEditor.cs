using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Loaders;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(CapabilityConfigScriptable))]
    public class CapabilityConfigScriptableEditor : UnityEditor.Editor
    {
        private CapabilityConfigScriptable scriptable;
        
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
            root.Add(new GoalList(this.scriptable, generator));

            root.Add(new Header("Actions"));
            root.Add(new ActionList(this.scriptable, generator));

            root.Add(new Header("WorldSensors"));
            root.Add(new SensorList<BehaviourWorldSensor>(this.scriptable, generator, this.scriptable.worldSensors));

            root.Add(new Header("TargetSensors"));
            root.Add(new SensorList<BehaviourTargetSensor>(this.scriptable, generator, this.scriptable.targetSensors));

            root.Add(new Header("MultiSensors"));
            root.Add(new SensorList<BehaviourMultiSensor>(this.scriptable, generator, this.scriptable.multiSensors));

            return root;
        }
        
    }
}