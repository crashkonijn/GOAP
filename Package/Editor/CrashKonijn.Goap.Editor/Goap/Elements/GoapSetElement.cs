using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Editor.Goap.OldElements;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.Elements
{
    public class GoapSetElement : IEditorElement
    {
        public VisualElement Render(EditorState state, Action onChange)
        {
            if (state.SelectedSetConfig == null)
                return null;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Packages/LamosInteractive.Goap.Unity/Editor/Goap/Views/GoapSet.uxml");
            var set = visualTree.Instantiate();
            
            var slot = set.Q("slot");
            
            slot.Add(new ReferencesElement<IGoalConfig>("Goals", state.SelectedSetConfig.Goals, element => new ReferenceElement.Data
            {
                Label = element.Name
            }).Render(state, onChange));
            
            // slot.Add(new ReferencesElement<ActionConfig>("Actions", state.SelectedSetConfig.actions, element => new ReferenceElement.Data
            // {
            //     Label = element.name
            // }).Render(state, onChange));
            
            slot.Add(new ReferencesElement<IWorldSensorConfig>("World Sensors", state.SelectedSetConfig.WorldSensors, element => new ReferenceElement.Data
            {
                Label = element.Name
            }).Render(state, onChange));
            
            slot.Add(new ReferencesElement<ITargetSensorConfig>("Target Sensors", state.SelectedSetConfig.TargetSensors, element => new ReferenceElement.Data
            {
                Label = element.Name
            }).Render(state, onChange));
            
            this.ValidateWorldKeys(slot, state, onChange);
            this.ValidateTargetKeys(slot, state, onChange);
            
            return set;
        }
        
        private void ValidateWorldKeys(VisualElement root, EditorState state, Action onChange)
        {
            var required = state.SelectedSetConfig.Actions.SelectMany(x => x.Conditions.Cast<Condition>().Select(y => y.worldKey).Concat(x.Effects.Cast<Effect>().Select(z => z.worldKey)));
            var provided = state.SelectedSetConfig.WorldSensors.Select(x => x.Key);

            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            root.Add(new AlertElement
            {
                Type = AlertElement.AlertType.Danger,
                Header = "Missing world keys",
                Text = string.Join(", ", missing.Select(x => x.Name))
            }.Render(state, onChange));
        }
        
        private void ValidateTargetKeys(VisualElement root, EditorState state, Action onChange)
        {
            var required = state.SelectedSetConfig.Actions.Select(x => x.Config.target);
            var provided = state.SelectedSetConfig.TargetSensors.Select(x => x.Key);

            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            root.Add(new AlertElement
            {
                Type = AlertElement.AlertType.Danger,
                Header = "Missing target keys",
                Text = string.Join(", ", missing.Select(x => x.Name))
            }.Render(state, onChange));
        }
    }
}