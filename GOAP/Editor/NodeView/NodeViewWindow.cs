/**
 * Copyright 2017 Sword GC
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Author: Peter Klooster | CrashKonijn
 * Project: https://github.com/crashkonijn/GOAP
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SwordGC.AI.Goap;

namespace SwordGC.AI.Core.Editor.NodeView
{
    public class NodeViewEditor : EditorWindow
    {
        /// <summary>
        /// Holds all nodes
        /// </summary>
        private List<Node> nodes;

        /// <summary>
        /// The default node style
        /// </summary>
        private GUIStyle nodeStyle;
        /// <summary>
        /// The active node style
        /// </summary>
        private GUIStyle activeStyle;
        /// <summary>
        /// The blocked node style
        /// </summary>
        private GUIStyle blockedStyle;

        /// <summary>
        /// The active agent for which the data is drawn
        /// </summary>
        private GoapAgent activeAgent;

        /// <summary>
        /// The dragged offset of the content
        /// </summary>
        private Vector2 offset;

        [MenuItem("Window/GOAP Visualizer")]
        private static void OpenWindow()
        {
            NodeViewEditor window = GetWindow<NodeViewEditor>();
            window.titleContent = new GUIContent("Node View Editor");
        }

        private void OnEnable()
        {
            LoadStyles();

            autoRepaintOnSceneChange = true;

            // Remove delegate listener if it has previously
            // been assigned.
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            // Add (or re-add) the delegate.
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        void OnDestroy()
        {
            // When the window is destroyed, remove the delegate
            // so that it will no longer do any drawing.
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }


        private void OnGUI()
        {
            FindAgentController();

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawDataSet();
            DrawNodes();
            DrawAgents();

            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (activeAgent == null || activeAgent.activeActions == null) return;

            Vector3 prevPos = activeAgent.transform.position;

            for (int i = 0; i < activeAgent.activeActions.Count; i++)
            {
                //Handles.ArrowHandleCap(i, activeAgent.activeActions[i].position, Quaternion.Euler(activeAgent.activeActions[i].position - prevPos), Vector3.Distance(prevPos, activeAgent.activeActions[i].position), EventType.Ignore);
                DrawArrow.ForDebug(prevPos, activeAgent.activeActions[i].position - prevPos);
                prevPos = activeAgent.activeActions[i].position;
            }


            // Do your drawing here using Handles.
            Handles.BeginGUI();
            // Do your drawing here using GUI.
            Handles.EndGUI();
        }

        /// <summary>
        /// Loads the styles
        /// </summary>
        private void LoadStyles ()
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            string[] tAssets = AssetDatabase.FindAssets("node_background_ t:Texture2D");

            foreach (string s in tAssets)
            {
                Texture2D tTex = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(s), typeof(Texture2D));
                textures.Add(tTex.name, tTex);
            }

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = textures["node_background_normal"];
            nodeStyle.border = new RectOffset(5, 5, 5, 5);
            nodeStyle.contentOffset = new Vector2(10, 10);
            nodeStyle.richText = true;

            activeStyle = new GUIStyle(nodeStyle);
            activeStyle.normal.background = textures["node_background_active"];

            blockedStyle = new GUIStyle(nodeStyle);
            blockedStyle.normal.background = textures["node_background_disabled"];
        }

        /// <summary>
        /// Draws a button for each agent in the scene
        /// </summary>
        private void DrawAgents ()
        {
            GoapAgent[] agents = FindObjectsOfType<GoapAgent>();

            GUIStyle tStyle = EditorStyles.toolbarButton;
            for (int i = 0; i < agents.Length; i++)
            {
                tStyle.normal.textColor = activeAgent == agents[i] ? Color.blue : Color.black;
                if (GUI.Button(new Rect(new Vector2((110 * i) + 20, 20), new Vector2(100, 20)), agents[i].transformName, tStyle))
                {
                    activeAgent = agents[i];
                    LoadNodes();
                }
            }
            tStyle.normal.textColor = Color.black;
        }
        
        /// <summary>
        /// Draws the current dataset
        /// </summary>
        private void DrawDataSet ()
        {
            if (activeAgent == null || activeAgent.dataSet == null) return;

            string s = "";

            foreach (KeyValuePair<string, bool> entry in activeAgent.dataSet.data)
            {
                s += "\n" + (entry.Value ? "<color=green>" : "<color=red>");
                s += (entry.Value ? "" : "!") + entry.Key;
                s += "</color>";
            }


            GUI.Box(new Rect(offset.x, 50f + offset.y, 200, activeAgent.dataSet.data.Count * 17), s, nodeStyle);
        }

        /// <summary>
        /// Draws all nodes
        /// </summary>
        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw(ActiveActions());
                }
            }
        }

        /// <summary>
        /// Returns the current active actions
        /// </summary>
        /// <returns></returns>
        GoapAction[] ActiveActions ()
        {
            if (activeAgent.activeActions != null)
            {
                return activeAgent.activeActions.ToArray();
            }
            else
            {
                return new GoapAction[0];
            }
        }

        /// <summary>
        /// Handles events
        /// </summary>
        /// <param name="e"></param>
        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        /// <summary>
        /// Handle draging
        /// </summary>
        /// <param name="delta"></param>
        private void OnDrag(Vector2 delta)
        {
            offset += delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        /// <summary>
        /// Find an agent
        /// </summary>
        private void FindAgentController ()
        {
            if (activeAgent == null)
            {
                GoapAgent[] agents = FindObjectsOfType<GoapAgent>();

                if (agents.Length > 0)
                {
                    activeAgent = agents[0];
                    LoadNodes();
                }
            }
            else if(nodes == null || nodes.Count == 0)
            {
                LoadNodes();
            }
            else if (activeAgent.editorDirty)
            {
                nodes.Clear();
                activeAgent.editorDirty = false;
            }
        }

        /// <summary>
        /// (Re)loades all nodes
        /// </summary>
        private void LoadNodes ()
        {
            offset = Vector2.zero;
            nodes = new List<Node>();

            if (activeAgent == null || activeAgent.goals == null) return;

            GoalNode prev = null;

            foreach (GoapGoal goal in activeAgent.goals.Values)
            {
                GoalNode node = new GoalNode(goal, 0, prev, activeAgent.dataSet, nodeStyle, activeStyle, blockedStyle);
                nodes.Add(node);
                prev = node;
            }
        }

        /// <summary>
        /// Draws the grid
        /// </summary>
        /// <param name="gridSpacing"></param>
        /// <param name="gridOpacity"></param>
        /// <param name="gridColor"></param>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }
    }
}