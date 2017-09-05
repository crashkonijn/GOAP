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

using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SwordGC.AI.Core.Editor.NodeView
{
    public class GoalNode : Node
    {
        /// <summary>
        /// The corresponding Goal
        /// </summary>
        GoapGoal goal;

        public override Rect rect
        {
            get
            {
                return new Rect(new Vector2(neighbourOffset, 150 * depth) + GetOffset, new Vector2(width, 100f));
            }
        }

        override public string title
        {
            get { return goal.key; }
        }

        private Vector2 GetOffset
        {
            get
            {
                return neighbour == null ? offset : new Vector2(0f, offset.y);
            }
        }

        public override string data
        {
            get
            {
                string s = goal.cheapestCost.ToString("0.0") + " * " + goal.multiplier + " [" + (goal.cheapestCost * goal.multiplier).ToString("0.0") + "]";
                return s;
            }
        }

        public GoalNode(GoapGoal goal, int depth, Node neighbour, DataSet dataSet, GUIStyle nodeStyle, GUIStyle activeStyle, GUIStyle blockedStyle) : base(null, depth, neighbour, dataSet, nodeStyle, activeStyle, blockedStyle)
        {
            this.goal = goal;

            AddChilds(goal.childs);
        }
    }
}
