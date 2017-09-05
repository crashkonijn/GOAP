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

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace SwordGC.AI.Goap
{
    [CustomEditor(typeof(GoapAgent), true)]
    public class GoapAgentEditor : Editor
    {
        /// <summary>
        /// This agent
        /// </summary>
        GoapAgent agent;

        void OnEnable()
        {
            agent = (GoapAgent)target;
        }

        public override void OnInspectorGUI()
        {

            if (agent == null) return;

            if(agent.possibleActions == null || agent.goals == null) return;

            EditorGUILayout.EnumPopup("Current thread", agent.runningThread);

            foreach (GoapAction action in agent.possibleActions)
            {
                DrawAvailableAction(action);
            }

            foreach (GoapGoal goal in agent.goals.Values)
            {
                DrawGoal(goal);
            }
        }

        public void OnSceneGUI()
        {
            //DrawPath();
            Repaint();
        }

        /// <summary>
        /// Draws the given goal
        /// </summary>
        /// <param name="goal"></param>
        private void DrawGoal (GoapGoal goal)
        {
            EditorGUILayout.LabelField(goal.GetType().Name);
            EditorGUILayout.BeginVertical("Box");
            foreach (GoapAction child in goal.childs)
            {
                DrawAction(child);
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the given action
        /// </summary>
        /// <param name="action"></param>
        private void DrawAction (GoapAction action)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(action.GetType().Name, EditorStyles.boldLabel);
            if (GUILayout.Button("delete"))
            {
                agent.RemoveAction(action);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Toggle("Preconditions Valid", action.preconditionsValid);
            EditorGUILayout.Toggle("Procedural Valid", action.proceduralConditionsValid);

            if (action.childs.Count > 0)
            {
                EditorGUILayout.BeginVertical("Box");
                foreach (GoapAction child in action.childs)
                {
                    DrawAction(child);
                }
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// Draws the given action
        /// </summary>
        /// <param name="action"></param>
        private void DrawAvailableAction (GoapAction action)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(action.GetType().Name, EditorStyles.boldLabel);
            if (GUILayout.Button("Add"))
            {
                agent.AddAction(action.Clone().GenerateNewGUID());
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPath ()
        {
            if (agent == null || agent.activeActions == null) return;

            Vector3 prevPos = agent.transform.position;

            for (int i = 0; i < agent.activeActions.Count; i++)
            {
                DrawArrow.ForDebug(prevPos, agent.activeActions[i].position - prevPos);
                prevPos = agent.activeActions[i].position;
            }
        }
    }
}