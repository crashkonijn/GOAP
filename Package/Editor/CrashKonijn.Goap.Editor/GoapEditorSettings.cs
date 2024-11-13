using System.IO;
using UnityEditor;

namespace CrashKonijn.Goap.Editor
{
    public static class GoapEditorSettings
    {
        public static string BasePath
        {
            get
            {
                var assets = AssetDatabase.FindAssets($"t:Script {nameof(GoapEditorSettings)}");
                
                // This should not happen, but who knows
                if (assets.Length == 0)
                    return "Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor";
                
                return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(assets[0]));
            }
        }
      
        public const string Version = "3.0.22-beta";
    }
}
