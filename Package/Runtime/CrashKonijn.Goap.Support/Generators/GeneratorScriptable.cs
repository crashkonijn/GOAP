using System.IO;
using CrashKonijn.Goap.Generators;
using UnityEngine;

namespace CrashKonijn.Goap.Support.Generators
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
    }
}