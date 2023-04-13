using System;
using System.Linq;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Editor.Elements;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoapSetConfigScriptable))]
    public class GoapSetConfigEditor : UnityEditor.Editor
    {
        private GoapSetConfigScriptable config;
        
        public override VisualElement CreateInspectorGUI()
        {
            this.config = (GoapSetConfigScriptable) this.target;
            
            var root = new VisualElement();
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.crashkonijn.goap/Editor/CrashKonijn.Goap.Editor/Styles/Generic.uss");
            root.styleSheets.Add(styleSheet);

            root.Add(this.Group("Goals and Actions", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("actions")));
                card.Add(new PropertyField(this.serializedObject.FindProperty("goals")));
            }));
            
            root.Add(this.Group("World Keys", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("worldSensors")));
                card.Add(this.SimpleLabelView("World keys", this.config.GetWorldKeys(), (label, key) =>
                {
                    label.text = key.Name;
                }));
                this.ValidateWorldKeys(card);
            }));
            
            root.Add(this.Group("Targets", card =>
            {
                card.Add(new PropertyField(this.serializedObject.FindProperty("targetSensors")));
                card.Add(this.SimpleLabelView("Target keys", this.config.GetTargetKeys(), (label, key) =>
                {
                    label.text = key.Name;
                }));
                this.ValidateTargetKeys(card);
            }));

            return root;
        }

        private VisualElement Group(string title, Action<Card> callback)
        {
            var group = new VisualElement();
            group.Add(new Header(title));
            group.Add(new Card(callback));
            return group;
        }

        private VisualElement SimpleLabelView<T>(string title, T[] list, Action<Label, T> bind)
        {
            var foldout = new Foldout()
            {
                text = title,
            };
            var listView = new ListView(list, 20, () => new Label())
            {
                bindItem = (element, index) =>
                {
                    bind(element as Label, list[index]);
                },
                selectionType = SelectionType.None
            };
            listView.AddToClassList("card");
            foldout.Add(listView);

            return foldout;
        }

        private void ValidateWorldKeys(VisualElement root)
        {
            var required = this.config.GetWorldKeys();
            var provided = this.config.WorldSensors.Select(x => x.Key);

            var missing = required.Except(provided).ToHashSet();
            
            if (!missing.Any())
                return;

            var helpBox = new HelpBox("World keys missing sensors:", HelpBoxMessageType.Error);
            helpBox.Add(new Label(string.Join(", ", missing.Select(x => x.Name))));
            root.Add(helpBox);
        }

        private void ValidateTargetKeys(VisualElement root)
        {
            var required = this.config.GetTargetKeys();
            var provided = this.config.TargetSensors.Select(x => x.Key);

            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;

            var helpBox = new HelpBox("World keys missing sensors:", HelpBoxMessageType.Error);
            helpBox.Add(new Label(string.Join(", ", missing.Select(x => x.Name))));
            root.Add(helpBox);
        }
    }
}