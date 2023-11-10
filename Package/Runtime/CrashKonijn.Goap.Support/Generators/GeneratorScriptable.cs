using System.IO;
using CrashKonijn.Goap.Generators;
using UnityEngine;

namespace CrashKonijn.Goap.Support.Generators
{
    [CreateAssetMenu(menuName = "Goap/Generator")]
    public class GeneratorScriptable : ScriptableObject
    {
        public void CreateGoal(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateGoal(assetPath, name, "CrashKonijn.Goap.GenTest");
        }
        
        public void CreateAction(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateAction(assetPath, name, "CrashKonijn.Goap.GenTest");
        }
        
        public void CreateTargetKey(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateTargetKey(assetPath, name, "CrashKonijn.Goap.GenTest");
        }
        
        public void CreateWorldKey(string name)
        {
            var generator = new ClassGenerator();
            
            var assetPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(this));
            
            generator.CreateWorldKey(assetPath, name, "CrashKonijn.Goap.GenTest");
        }
    }
}