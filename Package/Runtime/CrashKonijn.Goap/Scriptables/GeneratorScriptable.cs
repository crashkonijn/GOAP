using System.IO;
using CrashKonijn.Goap.Generators;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/Generator")]
    public class GeneratorScriptable : ScriptableObject
    {
        public string nameSpace = "CrashKonijn.Goap.GenTest";
        
        public GenerationResult CreateGoal(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            return generator.CreateGoal(assetPath, name, this.nameSpace);
        }
        
        public GenerationResult CreateAction(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            return generator.CreateAction(assetPath, name, this.nameSpace);
        }
        
        public GenerationResult CreateTargetKey(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            return generator.CreateTargetKey(assetPath, name, this.nameSpace);
        }
        
        public GenerationResult CreateWorldKey(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            return generator.CreateWorldKey(assetPath, name, this.nameSpace);
        }
        
        public GenerationResult CreateMultiSensor(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            return generator.CreateMultiSensor(assetPath, name, this.nameSpace);
        }

        public Scripts GetClasses() => ClassScanner.GetClasses(this.nameSpace,
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