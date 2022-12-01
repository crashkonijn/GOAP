using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Editor.Goap.Elements;
using CrashKonijn.Goap.Editor.Goap.OldElements;
using CrashKonijn.Goap.Scriptables;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap
{
    public class GoapEditorWindow : EditorWindow
    {
        private VisualElement m_RightPane;

        private EditorState state = new EditorState();

        [MenuItem("Tools/GOAP Editor")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<GoapEditorWindow>();
            wnd.titleContent = new GUIContent("GOAP Editor");

            // Limit size of the window
            wnd.minSize = new Vector2(450, 200);
            wnd.maxSize = new Vector2(1920, 720);
        }

        public void CreateGUI()
        {
            this.state = this.Load();
            this.Render();
        }

        private void Render()
        {
            this.rootVisualElement.Clear();

            this.rootVisualElement.Add(new ToolbarElement().Render(this.state, this.Render));

            switch (this.state.Page)
            {
                case GoapEditorPage.Actions:
                    this.RenderActions();
                    break;
                case GoapEditorPage.Goals:
                    this.RenderGoals();
                    break;
                case GoapEditorPage.Sets:
                    this.RenderSets();
                    break;
                case GoapEditorPage.Sensors:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RenderActions()
        {
            // var left = new ListViewElement<ActionConfig>(this.state.ActionConfigs, item =>
            // {
            //     this.state.SelectedActionConfig = item;
            //     this.Render();
            // });
            //
            // var right = new ScrollViewElement();
            //
            // rootVisualElement.Add(new SplitViewElement(left, right).Render(this.state, this.Render));
        }


        private void RenderGoals()
        {
            var left = new ListViewElement<IGoalConfig>(this.state.GoalConfigs, item =>
            {
                this.state.SelectedGoalConfig = item;
                this.Render();
            });
        
            var right = new ScrollViewElement(new GoalElement());

            this.rootVisualElement.Add(new SplitViewElement(left, right).Render(this.state, this.Render));
        }
    
        private void RenderSets()
        {
            var left = new ListViewElement<IGoapSetConfig>(this.state.SetConfigs, item =>
            {
                this.state.SelectedSetConfig = item;
                this.Render();
            });
        
            var right = new ScrollViewElement(new GoapSetElement());

            this.rootVisualElement.Add(new SplitViewElement(left, right).Render(this.state, this.Render));
        }

        private EditorState Load()
        {
            return new EditorState
            {
                ActionConfigs = this.Load<ActionBase>(),
                GoalConfigs = this.Load<GoalConfigScriptable>().Cast<IGoalConfig>().ToList(),
                SetConfigs = this.Load<GoapSetConfigScriptable>().Cast<IGoapSetConfig>().ToList(),
            };
        }

        private List<T> Load<T>()
            where T : UnityEngine.Object
        {
            var allObjectGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            return allObjectGuids.Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
        }

        public enum GoapEditorPage
        {
            Goals,
            Actions,
            Sensors,
            Sets
        }
    }
}