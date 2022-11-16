using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.OldElements
{
    public class GoalElement : IEditorElement
    {
        public VisualElement Render(EditorState state, Action onChange)
        {
            if (state.SelectedGoalConfig == null)
                return null;

            var box = new Box();
            
            box.Add(new Label
            {
                text = state.SelectedGoalConfig.name
            });

            var baseCost = new IntegerField("Base cost");

            baseCost.value = state.SelectedGoalConfig.baseCost;

            baseCost.RegisterValueChangedCallback(value => state.SelectedGoalConfig.baseCost = value.newValue);
            
            box.Add(baseCost);

            return box;
        }
    }
}