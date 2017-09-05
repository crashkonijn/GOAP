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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SwordGC.AI.Core.Editor.NodeView
{
    public class Node
    {
        /// <summary>
        /// Reference to the current dataset
        /// </summary>
        public DataSet dataSet;

        /// <summary>
        /// Set to true for smaller rendering (for more overview)
        /// </summary>
        public bool useSmallRect = false;
        /// <summary>
        /// The size when rendering big rects
        /// </summary>
        public Rect bigRect = new Rect(210, 60, 200, 100);
        /// <summary>
        /// The size when rendering small rects
        /// </summary>
        public Rect smallRect = new Rect(110, 20, 100, 50);
        /// <summary>
        /// Gets the correct size rect
        /// </summary>
        public Rect sizeRect
        {
            get
            {
                return useSmallRect ? smallRect : bigRect;
            }
        }

        /// <summary>
        /// The current offset
        /// </summary>
        public Vector2 offset = new Vector2(200, 50);
        /// <summary>
        /// The cached rect of this nide
        /// </summary>
        protected Rect cachedRect;
        /// <summary>
        /// The position rect
        /// </summary>
        public virtual Rect rect
        {
            get
            {
                return new Rect(new Vector2(neighbourOffset, parentOffset.y + sizeRect.height +50), new Vector2(width, sizeRect.height));
            }
        }

        /// <summary>
        /// The actual rect that needs to be drawn
        /// </summary>
        public virtual Rect drawRect
        {
            get
            {
                Vector2 pos = new Vector2(((rect.xMin + rect.xMax) / 2f) - (cachedRect.width / 2f), rect.yMin);
                return new Rect(pos, new Vector2(cachedRect.width, rect.height));
            }
        }

        /// <summary>
        /// The (bounds) rect containing all childs
        /// </summary>
        public virtual Rect childRect
        {
            get
            {
                if (childs.Count > 0)
                {
                    return new Rect(childs[0].drawnRect.min, new Vector2(childs[childs.Count-1].drawnRect.xMax - childs[0].drawnRect.xMin, childs[0].drawnRect.height));
                }
                else
                {
                    return Rect.zero;
                }
            }
        }

        /// <summary>
        /// The latest rect that was drawn
        /// </summary>
        public Rect drawnRect { get; protected set; }

        /// <summary>
        /// The title of this node
        /// </summary>
        public virtual string title { get; set; }
        /// <summary>
        /// The data for this node
        /// </summary>
        public virtual string data { get; set; }
        
        /// <summary>
        /// The default node style
        /// </summary>
        protected GUIStyle style;
        /// <summary>
        /// The active node style
        /// </summary>
        protected GUIStyle activeStyle;
        /// <summary>
        /// The blocked node style
        /// </summary>
        protected GUIStyle blockedStyle;

        /// <summary>
        /// All the child nodes
        /// </summary>
        public List<Node> childs = new List<Node>();

        /// <summary>
        /// The depth of this node
        /// </summary>
        public int depth;

        /// <summary>
        /// Parent node
        /// </summary>
        public Node parent;
        /// <summary>
        /// Neighbour node to the left
        /// </summary>
        public Node neighbour;

        /// <summary>
        /// The color of the text on this node
        /// </summary>
        protected virtual string textColor
        {
            get
            {
                return "white";
            }
        }

        /// <summary>
        /// The offset of the parent node
        /// </summary>
        public virtual Vector2 parentOffset
        {
            get
            {
                return parent != null ? parent.rect.min : Vector2.zero;
            }
        }

        /// <summary>
        /// The width of this node
        /// </summary>
        public float width
        {
            get
            {
                float f = 0f;
                
                for (int i = 0; i < childs.Count; i++)
                {
                    
                    f += childs[i].width;
                }

                return Mathf.Max(sizeRect.size.x + 10f, f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float neighbourOffset
        {
            get
            {
                return neighbour != null ? neighbour.rect.xMax : parentOffset.x;
            }
        }

        /// <summary>
        /// Gets the current needed style for this node
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        protected virtual GUIStyle GetStyle(GoapAction[] active)
        {
            return style;
        }

        public Node(Node parent, int depth, Node neighbour, DataSet dataSet, GUIStyle nodeStyle, GUIStyle activeStyle, GUIStyle blockedStyle)
        {
            cachedRect = new Rect((sizeRect.size .y + 10), sizeRect.position.y * depth, sizeRect.size.x, sizeRect.size.y);

            style = nodeStyle;
            this.activeStyle = activeStyle;
            this.blockedStyle = blockedStyle;
            this.depth = depth;
            this.neighbour = neighbour;
            this.parent = parent;
            this.dataSet = dataSet;
        }

        /// <summary>
        /// Updates the offset
        /// </summary>
        /// <param name="delta">The offset to be added</param>
        public void Drag(Vector2 delta)
        {
            offset += delta;

            foreach (Node c in childs)
            {
                c.Drag(delta);
            }
        }

        /// <summary>
        /// Debugs the (bounds) rect of this object
        /// </summary>
        /// <param name="tRect"></param>
        public void DebugRect (Rect tRect)
        {
            Handles.DrawLines(new Vector3[8] {
                    new Vector3(tRect.xMin, tRect.yMax),
                    new Vector3(tRect.xMin, tRect.yMin),
                    new Vector3(tRect.xMax, tRect.yMax),
                    new Vector3(tRect.xMax, tRect.yMin),
                    new Vector3(tRect.xMax, tRect.yMax),
                    new Vector3(tRect.xMin, tRect.yMax),
                    new Vector3(tRect.xMax, tRect.yMin),
                    new Vector3(tRect.xMin, tRect.yMin),
                });
        }

        /// <summary>
        /// Draws this node
        /// </summary>
        /// <param name="active"></param>
        public virtual void Draw(GoapAction[] active)
        {
            foreach (Node c in childs)
            {
                c.Draw(active);
            }

            Rect tRect = drawRect;

            if (childs.Count > 0)
            {
                Rect tChild = childRect;
                Vector2 pos = new Vector2(((tChild.xMin + tChild.xMax) / 2f) - (cachedRect.width / 2f), tRect.yMin);
                tRect.position = pos;
            }

            if(parent != null)
            {
                Handles.DrawBezier(
                    tRect.center + (Vector2.down * (tRect.height / 3f)),
                    parent.drawnRect.center - (Vector2.down * (tRect.height / 3f)),
                    tRect.center + (Vector2.down * 100f),
                    parent.drawnRect.center - (Vector2.down * 100f),
                    Color.white,
                    null,
                    2f
                );
            }
            
            drawnRect = tRect;
            
            if (useSmallRect)
                GUI.Box(tRect, "<color=" + textColor + ">" + title + "</color>", GetStyle(active));
            else
                GUI.Box(tRect, "<color=" + textColor + ">" + title + "\n" + data + "</color>", GetStyle(active));
        }

        /// <summary>
        /// Adds a child to this node
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public Node AddChilds (List<GoapAction> actions, int depth = 0)
        {
            actions = actions.OrderByDescending(x => x.childDepth).ToList();

            depth++;
            Node prev = null;
            foreach (GoapAction action in actions)
            {
                Node child = new ActionNode(this, depth, prev, action, dataSet, style, activeStyle, blockedStyle).AddChilds(action.childs, depth);
                prev = child;
                childs.Add(child);
            }
            return this;
        }
    }
}