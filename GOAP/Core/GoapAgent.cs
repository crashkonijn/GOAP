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

using CielaSpike;
using SwordGC.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwordGC.AI.Goap {
    public class GoapAgent : MonoBehaviour {

        /* ENUMS */
        public enum THREADTYPE { NONE, TREE_UPDATE, GOAP_UPDATE }
        public enum STATE { MOVING, ACTION }

        /* STATES */
        /// <summary>
        /// The current running thread type
        /// </summary>
        public THREADTYPE runningThread { get; private set; }
        /// <summary>
        /// The current state
        /// </summary>
        public STATE state { get; private set; }

        /* VARIABLES */
        /// <summary>
        /// Contains all current goals
        /// </summary>
        public Dictionary<string, GoapGoal> goals { get; protected set; }
        /// <summary>
        /// Contains all possible actions
        /// </summary>
        public List<GoapAction> possibleActions { get; protected set; }
        /// <summary>
        /// Contains all actions that need to be removed
        /// </summary>
        public List<GoapAction> removeActions { get; protected set; }
        /// <summary>
        /// Contains all actions that need to be added
        /// </summary>
        public List<GoapAction> addActions { get; protected set; }
        /// <summary>
        /// Can contain an action that needs to be performed before everything else when it's conditions are true
        /// </summary>
        protected GoapAction interveneAction;
        /// <summary>
        /// Can contain an action that needs to be performed when all other actions fail (idle)
        /// </summary>
        protected GoapAction idleAction;
        /// <summary>
        /// Contains the current active actions
        /// </summary>
        public List<GoapAction> activeActions { get; protected set; }
        /// <summary>
        /// Reference to the current dataset
        /// </summary>
        public DataSet dataSet { get; protected set; }
        /// <summary>
        /// Set to true when the node view editor needs updating
        /// </summary>
        public bool editorDirty;
        /// <summary>
        /// The name of this transform
        /// </summary>
        public string transformName { get; private set; }
        /// <summary>
        /// Holds the past X actions
        /// </summary>
        public Queue<GoapAction> actionHistory = new Queue<GoapAction>();

        /* GETTERS/SETTERS */
        /// <summary>
        /// Returns the current active action. It will return the idle action if there's none (can be null)
        /// </summary>
        public GoapAction ActiveAction
        {
            get { return activeActions != null && activeActions.Count > 0 ? activeActions[0] : idleAction; }
        }
        /// <summary>
        /// Returns true if the current action is in range
        /// </summary>
        protected bool ActiveActionInRange
        {
            get { return Vector3.Distance(transform.position, ActiveAction.target.transform.position) < ActiveAction.requiredRange; }
        }


        public virtual void Awake()
        {
            goals = new Dictionary<string, GoapGoal>();
            possibleActions = new List<GoapAction>();
            removeActions = new List<GoapAction>();
            addActions = new List<GoapAction>();

            dataSet = new DataSet();
            dataSet.SetData(GoapAction.Effects.HAS_OBJECT, false);
        }
        
        public virtual void Start() {
            transformName = transform.name;

            StartUpdatingTrees();
        }
        
        public virtual void FixedUpdate() {
            if (RunInterveneAction()) return;

            CheckThread();

            RunAction();
        }

        /// <summary>
        /// Runs the intervene action if it needs to
        /// </summary>
        /// <returns>True if the intervene action was run</returns>
        private bool RunInterveneAction ()
        {
            if (interveneAction != null)
            {
                interveneAction.Update(dataSet);

                if (interveneAction.preconditionsValid)
                {
                    state = STATE.ACTION;
                    interveneAction.Perform();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks the state of the threads, starts new threads as needed
        /// </summary>
        private void CheckThread ()
        {
            if (runningThread == THREADTYPE.NONE)
            {
                if (InternalRemoveActions() || InternalAddActions())
                {
                    StartUpdatingTrees();
                }
                else
                {
                    StartRunning();
                }
            }
        }

        /// <summary>
        /// Runs the current action
        /// </summary>
        private void RunAction ()
        {
            if (ActiveAction != null)
            {
                ActiveAction.Run(Time.deltaTime);

                if (ActiveAction.target != null && ActiveActionInRange)
                {
                    state = STATE.ACTION;
                    ActiveAction.Perform();
                }
                else
                {
                    state = STATE.MOVING;
                    Move(ActiveAction);
                }
            }
        }

        /// <summary>
        /// Starts the thread to update the trees
        /// </summary>
        private void StartUpdatingTrees ()
        {
            runningThread = THREADTYPE.TREE_UPDATE;
            this.StartCoroutineAsync(UpdateTrees());
        }

        /// <summary>
        /// Performs the tree updates, then merges with main thread
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateTrees ()
        {
            foreach (GoapGoal goal in goals.Values)
            {
                CreateTree(goal);
            }

            yield return Ninja.JumpToUnity;
            runningThread = THREADTYPE.NONE;
        }

        /// <summary>
        /// Creates the action tree for a goal
        /// </summary>
        /// <param name="goal">Starting point</param>
        void CreateTree(GoapGoal goal)
        {
            List<GoapAction> actions = GetMatchingGoalChilds(goal);

            goal.SetChilds(actions);

            editorDirty = true;
        }

        /// <summary>
        /// Get's all actions that fit as a child to the input action
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        List<GoapAction> GetMatchingActionChilds (GoapAction parent)
        {
            List<GoapAction> matches = new List<GoapAction>();

            // loop through all preconditions of the parent
            foreach (string condition in parent.preconditions.Keys)
            {
                // loop through all possible actions
                foreach (GoapAction action in possibleActions)
                {
                    // prevent it from endless looping
                    if (parent.GetType() == action.GetType()) continue;

                    // loop through all effects of the possible actions
                    foreach (string effect in action.effects.Keys)
                    {
                        // the keys and values are the same
                        if (condition == effect && parent.preconditions[condition] == action.effects[effect])
                        {
                            GoapAction tAction = action.Clone();

                            tAction.SetChilds(GetMatchingActionChilds(tAction));

                            matches.Add(tAction);
                        }
                    }
                }
            }

            return matches;
        }

        /// <summary>
        /// Returns a list of all actions that have the same goal
        /// </summary>
        /// <param name="parent">The parent goal</param>
        /// <returns>A list of (new) matching goals</returns>
        List<GoapAction> GetMatchingGoalChilds (GoapGoal parent)
        {
            List<GoapAction> matches = new List<GoapAction>();

            foreach (GoapAction action in possibleActions)
            {
                if (action.goal == parent.key)
                {
                    GoapAction tAction = action.Clone();

                    tAction.SetChilds(GetMatchingActionChilds(tAction));

                    matches.Add(tAction);
                }
            }

            return matches;
        }

        /// <summary>
        /// Marks an action for add
        /// </summary>
        /// <param name="action"></param>
        public void AddAction (GoapAction action)
        {
            addActions.Add(action);
        }

        /// <summary>
        /// Handles the adding of actions
        /// </summary>
        /// <returns></returns>
        private bool InternalAddActions ()
        {
            bool changed = false;
            foreach (GoapAction action in addActions)
            {
                if (!possibleActions.Contains(action))
                {
                    possibleActions.Add(action);
                    changed = true;
                }
            }

            addActions = new List<GoapAction>();
            return changed;
        }

        /// <summary>
        /// Marks an action for deletion
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction (GoapAction action)
        {
            removeActions.Add(action);
        }

        /// <summary>
        /// Handles the removing of actions
        /// </summary>
        /// <returns></returns>
        private bool InternalRemoveActions ()
        {
            bool changed = false;
            foreach (GoapAction rAction in removeActions)
            {
                foreach (GoapAction action in possibleActions)
                {
                    if (rAction.originalObjectGUID == action.originalObjectGUID)
                    {
                        possibleActions.Remove(action);
                        //Debug.Log("removed action: " + action.GetType().Name);
                        changed = true;
                        break;
                    }
                }
            }
            removeActions = new List<GoapAction>();
            return changed;
        }

        /// <summary>
        /// Updates all actions to the current dataset
        /// </summary>
        private void UpdateActions ()
        {
            foreach (GoapGoal goal in goals.Values)
            {
                goal.Update(dataSet);
            }
        }

        /// <summary>
        /// Starts a thread for GOAP calculations
        /// </summary>
        private void StartRunning ()
        {
            runningThread = THREADTYPE.GOAP_UPDATE;
            this.StartCoroutineAsync(RunThreaded(goals.Values.ToArray()));
        }

        /// <summary>
        /// Calls the calculations, then returns to the main thread
        /// </summary>
        /// <param name="goals"></param>
        /// <returns></returns>
        private IEnumerator RunThreaded (GoapGoal[] goals)
        {
            List<GoapAction> newActions = Run(goals);
            yield return Ninja.JumpToUnity;
            StartCoroutine(OnRunComplete(newActions));
        }

        /// <summary>
        /// Runs the calculations
        /// </summary>
        /// <param name="goals">Goals to be used</param>
        /// <returns>The new action path</returns>
        private List<GoapAction> Run(GoapGoal[] goals)
        {
            List<GoapAction> actions = null;
            float cheapest = Mathf.Infinity;

            float tCheapest = 0f;
            List<GoapAction> tActions;

            foreach (GoapGoal goal in goals)
            {
                tCheapest = goal.TotalCost(out tActions);
                if (tCheapest < cheapest)
                {
                    cheapest = tCheapest;
                    actions = tActions;
                }
            }

            return actions;
        }

        /// <summary>
        /// Called when Run() is completed
        /// </summary>
        /// <param name="newActions"></param>
        /// <returns></returns>
        private IEnumerator OnRunComplete (List<GoapAction> newActions)
        {
            float delay = (newActions != null && newActions.Count > 0) ? newActions[0].Delay : 0.05f;

            yield return new WaitForSeconds(delay);

            // end old actions
            GoapAction oldAction = (activeActions != null && activeActions.Count > 0) ? activeActions[0] : null;

            UpdateActions();
            activeActions = newActions;

            // start new action
            if (activeActions != null && activeActions.Count > 0 && oldAction != activeActions[0])
            {
                if(oldAction != null) oldAction.OnStop();
                activeActions[0].OnStart();

                actionHistory.Enqueue(activeActions[0]);

                if (actionHistory.Count > 10)
                {
                    actionHistory.Dequeue();
                }
            }

            runningThread = THREADTYPE.NONE;
        }
        
        /// <summary>
        /// Should be extended to include the move functionality
        /// </summary>
        /// <param name="nextAction"></param>
        protected virtual void Move (GoapAction nextAction)
        {

        }
    }
}