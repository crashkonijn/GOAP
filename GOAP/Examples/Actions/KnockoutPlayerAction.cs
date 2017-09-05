using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordGC.AI.Actions
{
    public class KnockoutPlayerAction : GoapAction
    {
        private int targetPlayerId;

        public KnockoutPlayerAction(GoapAgent agent, int targetPlayerId) : base(agent)
        {
            effects.Add(Effects.KNOCKED_OUT_PLAYER + targetPlayerId, true);
            
            requiredRange = 4f;
            cost = 50;

            this.targetPlayerId = targetPlayerId;
            this.targetString = "Player_" + targetPlayerId;
        }

        protected override bool CheckProceduralPreconditions(DataSet data)
        {
            // Check all procedural preconditions
            // Example:
            // if(!needAmmo || !ammoInRange) return false;

            return base.CheckProceduralPreconditions(data);
        }

        public override void Perform()
        {
            // Code to perform this action
        }

        public override GoapAction Clone()
        {
            return new KnockoutPlayerAction(agent, targetPlayerId).SetClone(originalObjectGUID);
        }
    }
}
