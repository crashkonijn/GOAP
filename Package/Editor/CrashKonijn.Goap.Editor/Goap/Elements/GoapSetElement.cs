using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
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
            
            slot.Add(new ReferencesElement<GoalConfig>("Goals", state.SelectedSetConfig.goals, element => new ReferenceElement.Data
            {
                Label = element.name
            }).Render(state, onChange));
            
            // slot.Add(new ReferencesElement<ActionConfig>("Actions", state.SelectedSetConfig.actions, element => new ReferenceElement.Data
            // {
            //     Label = element.name
            // }).Render(state, onChange));
            
            slot.Add(new ReferencesElement<WorldSensorConfig>("World Sensors", state.SelectedSetConfig.worldSensors, element => new ReferenceElement.Data
            {
                Label = element.name
            }).Render(state, onChange));
            
            slot.Add(new ReferencesElement<TargetSensorConfig>("Target Sensors", state.SelectedSetConfig.targetSensors, element => new ReferenceElement.Data
            {
                Label = element.name
            }).Render(state, onChange));
            
            this.ValidateWorldKeys(slot, state, onChange);
            this.ValidateTargetKeys(slot, state, onChange);
            
            return set;
        }
        
        private void ValidateWorldKeys(VisualElement root, EditorState state, Action onChange)
        {
            var required = state.SelectedSetConfig.actions.SelectMany(x => x.Conditions.Cast<Condition>().Select(y => y.worldKey).Concat(x.Effects.Cast<Effect>().Select(z => z.worldKey)));
            var provided = state.SelectedSetConfig.worldSensors.Select(x => x.key);

            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            root.Add(new AlertElement
            {
                Type = AlertElement.AlertType.Danger,
                Header = "Missing world keys",
                Text = string.Join(", ", missing.Select(x => x.name))
            }.Render(state, onChange));
        }
        
        private void ValidateTargetKeys(VisualElement root, EditorState state, Action onChange)
        {
            var required = state.SelectedSetConfig.actions.Select(x => x.Config.target);
            var provided = state.SelectedSetConfig.targetSensors.Select(x => x.key);

            var missing = required.Except(provided).ToHashSet();

            if (!missing.Any())
                return;
            
            root.Add(new AlertElement
            {
                Type = AlertElement.AlertType.Danger,
                Header = "Missing target keys",
                Text = string.Join(", ", missing.Select(x => x.name))
            }.Render(state, onChange));
        }
    }
}