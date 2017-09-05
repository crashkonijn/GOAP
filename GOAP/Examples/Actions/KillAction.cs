using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordGC.AI.Actions
{
    public class KillAction : GoapAction
    {
        private int targetPlayerId;

        public KillAction (GoapAgent agent, int targetPlayerId) : base(agent)
        {
            goal = GoapGoal.Goals.KILL_PLAYER + targetPlayerId;
            preconditions.Add(Effects.KNOCKED_OUT_PLAYER + targetPlayerId, true);
            
            requiredRange = 4f;
            cost = 20;

            this.targetPlayerId = targetPlayerId;

            targetString = "Player_" + targetPlayerId;
        }

        public override void Perform()
        {
            
        }

        public override GoapAction Clone()
        {
            return new KillAction(agent, targetPlayerId).SetClone(originalObjectGUID);
        }
    }
}