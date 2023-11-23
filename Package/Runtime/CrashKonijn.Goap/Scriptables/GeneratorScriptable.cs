using System.IO;
using CrashKonijn.Goap.Generators;
using CrashKonijn.Goap.Support.Loaders;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/Generator")]
    public class GeneratorScriptable : ScriptableObject
    {
        public string nameSpace = "CrashKonijn.Goap.GenTest";
        
        public void CreateGoal(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateGoal(assetPath, name, this.nameSpace);
        }
        
        public void CreateAction(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateAction(assetPath, name, this.nameSpace);
        }
        
        public void CreateTargetKey(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateTargetKey(assetPath, name, this.nameSpace);
        }
        
        public void CreateWorldKey(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateWorldKey(assetPath, name, this.nameSpace);
        }

        public Support.Loaders.Classes GetClasses() => ClassScanner.GetClasses(this.nameSpace,
            Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this)));

        public Script[] GetActions() => this.GetClasses().actions;
        public Script[] GetGoals() => this.GetClasses().goals;
        public Script[] GetTargetKeys() => this.GetClasses().targetKeys;
        public Script[] GetWorldKeys() => this.GetClasses().worldKeys;
        public Script[] GetTargetSensors() => this.GetClasses().targetSensors;
        public Script[] GetWorldSensors() => this.GetClasses().worldSensors;
        public Script[] GetMultiSensors() => this.GetClasses().multiSensors;
    }
}