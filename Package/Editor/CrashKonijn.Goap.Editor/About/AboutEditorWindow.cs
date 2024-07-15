using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class AboutEditorWindow : EditorWindow
    {
        private const int Version = 1;
        
        private static string goapSource = "loading";
        private static string goapVersion = "loading";
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
                card.Add(new Label(this.GetText())
                {
                    style =
                    {
                        whiteSpace = WhiteSpace.Normal,
                        overflow = Overflow.Hidden
                    }
                });
            }));

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
            if (goapVersion == "loading" || collectionsVersion == "loading")
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
            return @$"GOAP Version:                  {goapVersion} ({goapSource})
Unity Version:                   {Application.unityVersion}
Collections Version:         {collectionsVersion}";
        }

        private string GetText()
        {
            return $@"Thank you for trying out my GOAP package!

If you have any questions or need help, please don't hesitate to contact us on Discord or check out the documentation.

Please consider leaving a review on the Asset Store if you like it! 

I hope you enjoy using the GOAP!";
        }

        public void CopyDebug()
        {
            EditorGUIUtility.systemCopyBuffer = this.GetDebugText();
        }
        
        private static void CheckProgress()
        {
            if (request == null)
                request = Client.List();

            if (!request.IsCompleted)
                return;
            
            EditorApplication.update -= CheckProgress;

            goapSource = request.Result.Any(x => x.name == "com.crashkonijn.goap") ? "Git" : "Asset Store";
            goapVersion = GoapEditorSettings.Version;
            collectionsVersion = request.Result.Where(x => x.name == "com.unity.collections").Select(x => x.version).FirstOrDefault();

            instance.Render();
        }
    }
}