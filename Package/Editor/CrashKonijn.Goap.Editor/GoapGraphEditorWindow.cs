using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Editor.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Serializables;
using LamosInteractive.Goap.Debug;
using LamosInteractive.Goap.Interfaces;
using UnityEditor;
using UnityEngine;
using ICondition = CrashKonijn.Goap.Classes.ICondition;
using IEffect = CrashKonijn.Goap.Classes.IEffect;

namespace CrashKonijn.Goap.Editor
{
    public class GoapGraphEditorWindow : EditorWindow
    {
        private int width = 200;
        private int height = 150;
        private int marginX = 50;
        private int marginY = 50;
        
        private AgentBehaviour agent;

        private GUIStyle activeWindow;
        private GUIStyle inactiveWindow;
        private GUIStyle pathWindow;
        private GUIStyle disabledWindow;
        private Vector3 offset;

        [MenuItem("Window/Node editor")]
        static void ShowEditor() {
            var editor = EditorWindow.GetWindow<GoapGraphEditorWindow>();
            editor.Init();
        }

        private void Init()
        {

        }

        private void OnGUI()
        {
            this.DrawGrid(20, 0.2f, Color.gray);
            this.DrawGrid(100, 0.4f, Color.gray);
            
            if (!Application.isPlaying)
                return;

            this.LoadStyles();
            
            this.agent = Selection.activeGameObject?.GetComponent<AgentBehaviour>();

            if (this.agent == null)
                return;

            var data = this.agent.GoapSet.GetDebugGraph();

            var debugger = new GraphDebugger(data.Actions.Cast<IAction>().Concat(data.Goals).ToHashSet(), data.Config.ConditionObserver, data.Config.CostObserver, data.Config.KeyResolver);
            var graph = debugger.GetGraph();
            
            var nodes = new DebugGraph(graph).GetGraph(graph.RootNodes.Values.First(x => x.Action == this.agent.CurrentGoal));
            
            foreach (var (depth, renderNodes) in nodes.DepthNodes)
            {
                this.SetPositions(depth, renderNodes, nodes.MaxWidth);
            }
            
            
            // DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows
            //
            this.BeginWindows();

            var id = 0;
            
            foreach (var (key, value) in nodes.AllNodes)
            {
                foreach (var guid in value.Node.Conditions.SelectMany(x => x.Connections))
                {
                    this.DrawNodeCurve(value, nodes.Get(guid));
                }
            }
            
            foreach (var (key, value) in nodes.AllNodes)
            {
                GUI.Window(id++, value.Rect, _ => this.DrawNodeWindow(value),  value.Node.Action.GetType().Name, this.GetStyle(value));
            }

            this.EndWindows();
        }
        
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(this.position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(this.position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            Vector3 newOffset = new Vector3(this.offset.x % gridSpacing, this.offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, this.position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(this.position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private GUIStyle GetStyle(RenderNode node)
        {
            var action = node.Node.Action;

            if (action is IGoalBase)
                return this.pathWindow;

            if (action == this.agent.CurrentAction)
                return this.activeWindow;

            if (this.agent.CurrentActionPath.Contains(action))
                return this.pathWindow;

            return this.inactiveWindow;
        }

        private void SetPositions(int depth, List<RenderNode> renderNodes, int maxWidth)
        {
            for (var i = 0; i < renderNodes.Count; i++)
            {
                this.SetPosition(depth, renderNodes[i], maxWidth, i, renderNodes.Count);
            }
        }

        private void SetPosition(int depth, RenderNode renderNode, int maxWidth, int index, int rowCount)
        {
            var gridSize = ((maxWidth * this.width) + ((maxWidth - 1) * this.marginX)) / rowCount;
            var offset = (gridSize + this.marginX) / 2f;

            renderNode.Position = new Vector2((gridSize * index) + offset, depth * this.height) + new Vector2(this.marginX * index, this.marginY * depth) + new Vector2(100, 100);
        }

        private void DrawNodeWindow(RenderNode node)
        {
            var action = node.Node.Action as IActionBase;

            if (action == null)
                return;

            var conditionObserver = this.agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(this.agent.WorldData);

            var conditions = action.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));
            var effects = action.Effects.Select(x => this.GetText(x as IEffect, conditionObserver.IsMet(x)));

            var text = $"Target: {this.agent.WorldData.GetTarget(action).Position}\n\nEffects:\n{string.Join("\n", effects)}\nConditions:\n{string.Join("\n", conditions)}\n";

            GUIStyle style = new GUIStyle ();
            style.richText = true;
            style.normal.textColor = Color.white;
            
            GUI.Label(new Rect(10, 30, node.Rect.width - 10, node.Rect.height -10), text, style);
            
            GUI.DragWindow();
        }
        
        public void OnInspectorUpdate()
        {
            // This will only get called 10 times per second.
            this.Repaint();
        }

        private string GetText(ICondition condition, bool value)
        {
            var suffix = condition.Positive ? "true" : "false";
            var color = value ? "green" : "red";

            return $"    <color={color}>{condition.WorldKey.Name} ({suffix})</color>";
        }

        private string GetText(IEffect effect, bool value)
        {
            var suffix = effect.Positive ? "true" : "false";
            var color = value ? "green" : "red";

            return $"    <color={color}>{effect.WorldKey.Name} ({suffix})</color>";
        }

        void DrawNodeCurve(RenderNode start, RenderNode end)
        {
            Vector3 startPos = new Vector3(start.Rect.x + (start.Rect.width / 2f), start.Rect.y + start.Rect.height, 0);
            Vector3 endPos = new Vector3(end.Rect.x + (start.Rect.width / 2f), end.Rect.y, 0);
            Vector3 startTan = startPos + Vector3.up * 50;
            Vector3 endTan = endPos + Vector3.down * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

            var center = this.BezierPoint(0.5f, startPos, endPos, startTan, endTan);
            var size = new Vector2(50f, 25f);
            
            GUI.Box(new Rect((Vector2)center - (size / 2f) - new Vector2(0, 12.5f), size), Math.Round(this.GetDistance(start, end), 1).ToString());
        }

        private float GetDistance(RenderNode start, RenderNode end)
        {
            if (start.Node.Action is not IActionBase startAction)
                return 0f;
            
            if (end.Node.Action is not IActionBase endAction)
                return 0f;

            return Vector3.Distance(this.agent.WorldData.GetTarget(startAction).Position, this.agent.WorldData.GetTarget(endAction).Position);
        }
        
        private void LoadStyles()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            string[] tAssets = AssetDatabase.FindAssets("node_background_ t:Texture2D");

            foreach (string s in tAssets)
            {
                Texture2D tTex = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(s), typeof(Texture2D));
                textures.Add(tTex.name, tTex);
            }

            this.inactiveWindow = new GUIStyle
            {
                normal =
                {
                    textColor = Color.white,
                    background = textures["node_background_normal"]
                },
                border = new RectOffset(5, 5, 5, 5),
                contentOffset = new Vector2(10, 10),
                richText = true
            };

            this.activeWindow = new GUIStyle(this.inactiveWindow)
            {
                normal =
                {
                    background = textures["node_background_active"]
                }
            };

            this.pathWindow = new GUIStyle(this.inactiveWindow)
            {
                normal =
                {
                    background = textures["node_background_path"]
                }
            };

            this.disabledWindow = new GUIStyle(this.inactiveWindow)
            {
                normal =
                {
                    background = textures["node_background_disabled"]
                }
            };
        }

        private void InitNode()
        {
            
        }
        
        private Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 point = Vector3.zero;

            float u = 1.0f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += ttt * p3;

            return point;
        }
    }
}
