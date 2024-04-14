using System.Linq;
using CrashKonijn.Goap.Editor.Elements;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.About
{
    public class AboutEditorWindow : EditorWindow
    {
        private static string version = "loading";
        private static string collectionsVersion = "loading";
        
        private static ListRequest request;
        private static AboutEditorWindow instance;
        
        [MenuItem("Tools/GOAP/About")]
        private static void ShowWindow()
        {
            var window = GetWindow<AboutEditorWindow>();
            window.titleContent = new GUIContent("GOAP (About)");
            window.Show();
            
            instance = window;
            
            CheckProgress();
            
            EditorApplication.update += CheckProgress;
        }
        
        private void OnFocus()
        {
            instance = GetWindow<AboutEditorWindow>();
            
            CheckProgress();
            this.Render();
        }

        private void Render()
        {
            this.rootVisualElement.Clear();
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/Generic.uss");
            this.rootVisualElement.styleSheets.Add(styleSheet);
            
            styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{GoapEditorSettings.BasePath}/Styles/About.uss");
            this.rootVisualElement.styleSheets.Add(styleSheet);
            
            // Add image
            this.rootVisualElement.Add(new Image
            {
                image = AssetDatabase.LoadAssetAtPath<Texture2D>(GoapEditorSettings.BasePath + "/Textures/goap_header.png")
            });
            
            var scrollView = new ScrollView();
            scrollView.verticalScrollerVisibility = ScrollerVisibility.Auto;

            scrollView.Add(new Card((card) =>
            {
                card.Add(new Label(this.GetDebugText()));
                
                this.AddButtons(card);
            }));

            scrollView.Add(new Card((card) =>
            {
                this.AddLink(card, "Documentation", "https://goap.crashkonijn.com");
                this.AddLink(card, "Discord", "https://discord.gg/dCPnHaYNrm");
                this.AddLink(card, "Asset Store", "https://assetstore.unity.com/packages/slug/252687");
                this.AddLink(card, "GitHub", "https://github.com/crashkonijn/GOAP");
                
                this.AddLink(card, "Tutorials", "https://www.youtube.com/playlist?list=PLZWmMt_TbeYeatHa9hntDPu4zGEBAFffn");
                this.AddLink(card, "General References", "https://www.youtube.com/playlist?list=PLZWmMt_TbeYdBZKvlsRuuOubPTTfPuZot");
            }));
            
            this.rootVisualElement.Add(scrollView);
        }
        
        private void AddLink(VisualElement parent, string text, string url)
        {
            parent.Add(new Button(() =>
            {
                Application.OpenURL(url);
            })
            {
                text = text
            });
        }

        private void AddButtons(Card card)
        {
            if (version == "loading" || collectionsVersion == "loading")
            {
                card.Add(new Button(CheckProgress)
                {
                    text = "Refresh"
                });
                
                return;
            }

            card.Add(new Button(this.CopyDebug)
            {
                text = "Copy debug to Clipboard"
            });
        }
        
        private string GetDebugText()
        {
            var debug = $"GOAP Version:               {version}\nUnity Version:                {Application.unityVersion}\nCollections Version:       {collectionsVersion}";

            return debug;
        }

        public void CopyDebug()
        {
            EditorGUIUtility.systemCopyBuffer = this.GetDebugText();
        }
        
        private static void CheckProgress()
        {
            if (request == null)
            {
                request = Client.List();
            }
            
            if (request.IsCompleted)
            {
                EditorApplication.update -= CheckProgress;
                
                version = request.Result.Where(x => x.name == "com.crashkonijn.goap").Select(x => x.version).FirstOrDefault();
                collectionsVersion = request.Result.Where(x => x.name == "com.unity.collections").Select(x => x.version).FirstOrDefault();

                instance.Render();
            }
        }
    }
}