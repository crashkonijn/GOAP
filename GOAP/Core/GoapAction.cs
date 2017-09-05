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
    public class GoapAction
    {
        /// <summary>
        /// The (custum) GUID of the original where this instance was cloned from.
        /// Can be used to compare to different instances of the same 'object'
        /// </summary>
        public string originalObjectGUID { get; private set; }

        /// <summary>
        /// The GOAL that this action completes, can be empty
        /// </summary>
        public string goal { get; protected set; }
        /// <summary>
        /// The preconditions of this action
        /// </summary>
        public Dictionary<string, bool> preconditions { get; protected set; }
        /// <summary>
        /// The effects this action has
        /// </summary>
        public Dictionary<string, bool> effects { get; protected set; }
        /// <summary>
        /// The cost of this action
        /// </summary>
        public float cost { get; protected set; }
        /// <summary>
        /// The cached cheapest cost
        /// </summary>
        public float cheapestCost { get; protected set; }
        /// <summary>
        /// How much delay should be aplied before performing this action (value between 0 (slow) and 1 (fast))
        /// </summary>
        protected float delay = 0.5f;
        /// <summary>
        /// The delay in seconds when the delay value == 0
        /// </summary>
        protected float delaySlow = 0.3f;
        /// <summary>
        /// The delay in seconds when the delay value == 1
        /// </summary>
        protected float delayFast = 0.1f;
        /// <summary>
        /// The calculated amount of delay
        /// </summary>
        public float Delay
        {
            get
            {
                return Mathf.Lerp(delaySlow, delayFast, delay);
            }
        }
        /// <summary>
        /// The maximum amount of runtime for this action before it get's blocked for X seconds
        /// </summary>
        protected float maxRunTime = 3f;
        /// <summary>
        /// The current running time of this action
        /// </summary>
        protected float currentRunTime = 0f;
        /// <summary>
        /// Is this action blocked?
        /// </summary>
        public bool isBlocked { get; protected set; }
        /// <summary>
        /// Is this action running
        /// </summary>
        protected bool isRunning = false;

        /// <summary>
        /// The range required
        /// </summary>
        public float requiredRange { get; protected set; }
        /// <summary>
        /// The current position of this action
        /// </summary>
        public Vector3 position { get; protected set; }
        /// <summary>
        /// The target of this object
        /// </summary>
        public GameObject target { get; protected set; }
        /// <summary>
        /// The string that is the (unique) name of the target
        /// </summary>
        protected string targetString = "";
        /// <summary>
        /// The name of the target
        /// </summary>
        public string targetName
        {
            get { return target != null ? target.name : "No target"; }
        }
        /// <summary>
        /// Should this action be removed when there's no target?
        /// </summary>
        public bool removeWhenTargetless = false;

        /// <summary>
        /// Contains all the 'tree' children
        /// </summary>
        public List<GoapAction> childs;
        /// <summary>
        /// Contains branch of the cheapest childs
        /// </summary>
        public List<GoapAction> cheapestChilds;
        /// <summary>
        /// The agent that this action belongs to
        /// </summary>
        public GoapAgent agent { get; protected set; }
        /// <summary>
        /// The cached agentPosition
        /// </summary>
        public Vector3 agentPosition { get; protected set; }
        /// <summary>
        /// The cached result of CheckProceduralPreconditions()
        /// </summary>
        public bool proceduralConditionsValid { get; protected set; }
        /// <summary>
        /// The cached result of CheckPreconsitions();
        /// </summary>
        public bool preconditionsValid { get; protected set; }

        /// <summary>
        /// The amount of child 'layers' that are child of this object
        /// </summary>
        public int childDepth
        {
            get
            {
                if (childs.Count > 0)
                {
                    int deepest = 0;

                    foreach (GoapAction child in childs)
                    {
                        if (child.childDepth > deepest) deepest = child.childDepth;
                    }

                    return deepest + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The cached description of this object
        /// </summary>
        protected string description = "";
        /// <summary>
        /// Returns the description, return the type if there's none
        /// </summary>
        public string Description
        {
            get
            {
                return description == "" ? GetType().Name : description;
            }
        }

        /// <summary>
        /// The total distance between this action and the next wil be multiplied by this value
        /// </summary>
        protected float distanceMultiplier = 1f;
        /// <summary>
        /// The cached distance
        /// </summary>
        private float distance = 0f;
        /// <summary>
        /// Should be used to set the distance, returns the distance * distanceMultiplier
        /// </summary>
        public float Distance
        {
            get {
                return distance * distanceMultiplier;
            }
            set
            {
                distance = value;
            }
        }
        /// <summary>
        /// Returns the 'real' distance, AKA the distance withouth the multiplier
        /// </summary>
        public float RealDistance
        {
            get
            {
                return distance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent">The agend this action belongs to</param>
        /// <param name="delay">The amount of delay for this action (0f-1f)</param>
        public GoapAction(GoapAgent agent, float delay = 1f)
        {
            childs = new List<GoapAction>();
            preconditions = new Dictionary<string, bool>();
            effects = new Dictionary<string, bool>();

            this.agent = agent;
            this.delay = delay;

            GenerateNewGUID();
        }

        /// <summary>
        /// Generates a new GUID that can be used to identify copies of eachother
        /// </summary>
        /// <returns></returns>
        public GoapAction GenerateNewGUID ()
        {
            originalObjectGUID = System.Guid.NewGuid().ToString();
            return this;
        }

        /// <summary>
        /// Sets the childs to the input actions
        /// </summary>
        /// <param name="actions">The (new) childs</param>
        public void SetChilds(List<GoapAction> actions)
        {
            childs = actions;
        }

        /// <summary>
        /// Calculates the distance between this action and a child
        /// </summary>
        /// <param name="child">The child action</param>
        /// <returns>Return the distance</returns>
        protected virtual float DistanceToChild (GoapAction child)
        {
            return Vector3.Distance(position, child.position);
        }

        /// <summary>
        /// Returns the total cost of the cheapest branch of this actions
        /// Outputs a list containing the cheapest actions to perform this action
        /// </summary>
        /// <param name="cheapest">Cheapest actions to perform this action</param>
        /// <returns>Cheapest cost</returns>
        public float TotalCost (out List<GoapAction> cheapest)
        {
            cheapest = null;
            cheapestCost = Mathf.Infinity;
            float tCost = 0f;

            // No need to check if the procedural conditions are invalid
            if (proceduralConditionsValid && !preconditionsValid)
            {
                List<GoapAction> tAction = null;
                foreach (GoapAction child in childs)
                {
                    // Continue if this child is invalid
                    if (!child.proceduralConditionsValid) continue;

                    // Get and cache the distance to the child
                    child.Distance = DistanceToChild(child);
                    tCost = child.Distance + child.TotalCost(out tAction);

                    // This childs cost is low then the cheapest we've found yet
                    if (tCost < cheapestCost)
                    {
                        if (tAction != null)
                        {
                            cheapest = tAction;
                            cheapest.Add(this);
                        }
                        else
                        {
                            cheapest = tAction;
                        }
                        cheapestCost = tCost;
                    }
                }
            }

            // We didn't find any valid childs actions
            if (cheapest == null)
            {
                // Check if this action itself is completely valid
                // return this
                if (proceduralConditionsValid && preconditionsValid)
                {
                    cheapest = new List<GoapAction>();
                    cheapest.Add(this);

                    cheapestChilds = cheapest;
                    return cost + Vector3.Distance(position, agentPosition);
                }
                else
                {
                    cheapestChilds = cheapest;
                    return Mathf.Infinity;
                }
            }

            cheapestChilds = cheapest;
            return cost + cheapestCost;
        }

        /// <summary>
        /// Searches for the target if it's not set
        /// </summary>
        public virtual void UpdateTarget ()
        {
            if (targetString == "")
            {
                targetString = agent.gameObject.name;
            }
            if (target == null) target = GameObject.Find(targetString);
            if (removeWhenTargetless && target == null) agent.RemoveAction(this);
        }

        /// <summary>
        /// Sets the target GameObject
        /// </summary>
        /// <param name="target">The new target object</param>
        /// <returns>The same object</returns>
        public virtual GoapAction SetTarget (GameObject target)
        {
            this.target = target;
            targetString = target.name;
            return this;
        }

        /// <summary>
        /// Sets the target string
        /// </summary>
        /// <param name="targetString">The (unique) name of the target</param>
        /// <returns>The same object</returns>
        public virtual GoapAction SetTarget (string targetString)
        {
            this.targetString = targetString;
            return this;
        }

        /// <summary>
        /// Updates the positions of relevant objects
        /// </summary>
        public virtual void UpdatePosition ()
        {
            if (agent != null)
            {
                agentPosition = agent.transform.position;
            }
            if (target != null)
            {
                position = target.transform.position;
            }
        }

        /// <summary>
        /// Checks if all preconditions are true
        /// </summary>
        /// <param name="data">The dataset it needs to be compared to</param>
        /// <returns>True if all preconditions are true</returns>
        protected bool CheckPreconditions (DataSet data)
        {
            foreach (string key in preconditions.Keys)
            {
                if (!data.Equals(key, preconditions[key])) return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the procedural conditions of this action
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual bool CheckProceduralPreconditions(DataSet data)
        {
            return target != null && !isBlocked;
        }

        /// <summary>
        /// Updates this action, caches all the data
        /// </summary>
        /// <param name="data"></param>
        public virtual void Update(DataSet data)
        {
            UpdateTarget();
            UpdatePosition();

            preconditionsValid = CheckPreconditions(data);
            proceduralConditionsValid = CheckProceduralPreconditions(data);

            foreach (GoapAction child in childs)
            {
                child.Update(data);
            }
        }

        /// <summary>
        /// Get's called every fixed update that this action is active
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Run (float deltaTime)
        {
            if (isRunning)
            {
                currentRunTime += deltaTime;

                if (currentRunTime >= maxRunTime && !isBlocked)
                {
                    agent.StartCoroutine(BlockAction());
                }
            }

        }

        /// <summary>
        /// Get's called when this action is started
        /// </summary>
        public virtual void OnStart ()
        {
            //Debug.Log("OnStart: " + GetType().Name);
            isRunning = true;
        }

        /// <summary>
        /// Gets called when this action is being stopped
        /// </summary>
        public virtual void OnStop()
        {
            //Debug.Log("<color=blue>OnStop: " + GetType().Name + " [" + currentRunTime + "]</color>");
            isRunning = false;
            currentRunTime = 0f;
        }

        public virtual void Perform ()
        {
            throw new System.NotImplementedException("Perform is not implemented on " + this.GetType());
        }

        /// <summary>
        /// Needs to be called to clone this object, should be implemented in each clone
        /// </summary>
        /// <returns></returns>
        public virtual GoapAction Clone ()
        {
            throw new System.NotImplementedException("Clone is not implemented on " + this.GetType());
        }

        /// <summary>
        /// Makes this object a clone, sets GUID to original
        /// </summary>
        /// <param name="originalID">GUID of the original</param>
        /// <returns>Returns this object</returns>
        public GoapAction SetClone (string originalID)
        {
            originalObjectGUID = originalID;
            return this;
        }

        /// <summary>
        /// Blocks this action for X seconds
        /// </summary>
        /// <returns></returns>
        public IEnumerator BlockAction (float timeout = 1f)
        {
            isBlocked = true;
            yield return new WaitForSeconds(timeout);
            isBlocked = false;
        }

        /// <summary>
        /// This can be used to store easy constants, included constants are examples
        /// </summary>
        public class Effects
        {
            public const string KNOCKED_OUT_PLAYER = "IsKnockedOut";
            public const string HAS_OBJECT = "HasObject";
            public const string IS_GONER = "IsGoner";
            public const string SAME_TEAM = "SameTeam";
            public const string DASHED_AT = "DashedAt";
        }
    }
}
