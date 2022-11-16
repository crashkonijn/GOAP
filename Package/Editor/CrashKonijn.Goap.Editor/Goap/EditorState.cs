using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Editor.Goap
{
    public class EditorState
    {
        public GoapEditorWindow.GoapEditorPage Page { get; set; } = GoapEditorWindow.GoapEditorPage.Sets;
        
        public ActionConfig SelectedActionConfig { get; set; }
        public List<IActionBase> Actions { get; set; } = new List<IActionBase>();
        public List<ActionBase> ActionConfigs { get; set; } = new List<ActionBase>();
        public List<GoalConfig> GoalConfigs { get; set; }
        public GoalConfig SelectedGoalConfig { get; set; }
        public List<GoapSetConfig> SetConfigs { get; set; }
        public GoapSetConfig SelectedSetConfig { get; set; }
    }
}