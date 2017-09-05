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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordGC.AI.Goap
{
    public class GoapGoal
    {
        /// <summary>
        /// The unique name of this goal
        /// </summary>
        public string key { get; protected set; }

        /// <summary>
        /// Contains a list with all children
        /// </summary>
        public List<GoapAction> childs { get; protected set; }
        /// <summary>
        /// Contains branch of the cheapest childs
        /// </summary>
        public List<GoapAction> cheapestChilds;
        /// <summary>
        /// The cost of the cheapest branch
        /// </summary>
        public float cheapestCost { get; protected set; }
        /// <summary>
        /// The cost multiplier. Can be used to create a tendency towards a goal
        /// </summary>
        public float multiplier { get; protected set; }

        public GoapGoal (string key, float multiplier = 1f)
        {
            childs = new List<GoapAction>();

            this.key = key;
            this.multiplier = multiplier;
        }

        /// <summary>
        /// Sets the childs of this goal
        /// </summary>
        /// <param name="actions">The new childs</param>
        public void SetChilds (List<GoapAction> actions)
        {
            childs = actions;
        }

        /// <summary>
        /// Calculates the cheapest branch for this goal
        /// </summary>
        /// <param name="cheapest">The cheapest branch</param>
        /// <returns>The cheapest cost</returns>
        public float TotalCost(out List<GoapAction> cheapest)
        {
            cheapest = null;
            cheapestCost = Mathf.Infinity;
            float tCost = 0f;

            List<GoapAction> tAction = null;
            foreach (GoapAction child in childs)
            {
                tCost = child.TotalCost(out tAction);

                if (tCost < cheapestCost)
                {
                    if (tAction != null)
                    {
                        cheapest = tAction;
                    }
                    cheapestCost = tCost;
                }
            }

            return cheapest != null ? cheapestCost * multiplier : Mathf.Infinity;
        }

        /// <summary>
        /// Implement to update the multiplier of this goal
        /// </summary>
        /// <param name="data"></param>
        public virtual void UpdateMultiplier (DataSet data)
        {

        }

        /// <summary>
        /// Updates data on this goal and all childs
        /// </summary>
        /// <param name="data"></param>
        public void Update (DataSet data)
        {
            UpdateMultiplier(data);

            foreach (GoapAction child in childs)
            {
                child.Update(data);
            }
        }

        /// <summary>
        /// This can be used to store easy constants, included constants are examples
        /// </summary>
        public class Goals
        {
            public const string KILL_PLAYER = "KillPlayer";
            public const string STAY_ALIVE = "StayAlive";
        }
    }
}
