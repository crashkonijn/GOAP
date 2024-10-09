using System.IO;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [CreateAssetMenu(menuName = "Goap/Generator")]
    public class GeneratorScriptable : ScriptableObject
    {
        private readonly ClassGenerator generator = new();

        public string nameSpace = "CrashKonijn.Goap.GenTest";

        [SerializeField]
        public Scripts scripts = new();

#if UNITY_EDITOR
        public Script CreateGoal(string name)
        {
            var assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            return this.generator.CreateGoal(assetPath, name, this.nameSpace);
        }

        public Script CreateAction(string name)
        {
            var assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            return this.generator.CreateAction(assetPath, name, this.nameSpace);
        }

        public Script CreateTargetKey(string name)
        {
            var assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            return this.generator.CreateTargetKey(assetPath, name, this.nameSpace);
        }

        public Script CreateWorldKey(string name)
        {
            var assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            return this.generator.CreateWorldKey(assetPath, name, this.nameSpace);
        }

        public Script CreateMultiSensor(string name)
        {
            var assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            return this.generator.CreateMultiSensor(assetPath, name, this.nameSpace);
        }
#endif

        public Scripts GetClasses()
        {
#if UNITY_EDITOR

            this.scripts = ClassScanner.GetClasses(this.nameSpace, Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)));
            EditorUtility.SetDirty(this);
            // UnityEditor.AssetDatabase.SaveAssets();
#endif
            return this.scripts;
        }

        public Script[] GetActions() => this.GetClasses().actions;
        public Script[] GetGoals() => this.GetClasses().goals;
        public Script[] GetTargetKeys() => this.GetClasses().targetKeys;
        public Script[] GetWorldKeys() => this.GetClasses().worldKeys;
        public Script[] GetTargetSensors() => this.GetClasses().targetSensors;
        public Script[] GetWorldSensors() => this.GetClasses().worldSensors;
        public Script[] GetMultiSensors() => this.GetClasses().multiSensors;
    }
}
