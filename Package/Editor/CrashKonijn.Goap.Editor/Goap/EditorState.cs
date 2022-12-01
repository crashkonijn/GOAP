using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
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
        public List<IGoalConfig> GoalConfigs { get; set; }
        public IGoalConfig SelectedGoalConfig { get; set; }
        public List<IGoapSetConfig> SetConfigs { get; set; }
        public IGoapSetConfig SelectedSetConfig { get; set; }
    }
}