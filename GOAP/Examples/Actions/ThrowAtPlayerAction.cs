using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordGC.AI.Actions
{
    public class ThrowAtPlayerAction : GoapAction
    {
        private int targetPlayerId;

        public ThrowAtPlayerAction(GoapAgent agent, int targetPlayerId) : base(agent)
        {
            preconditions.Add(Effects.HAS_OBJECT, true);
            effects.Add(Effects.KNOCKED_OUT_PLAYER + targetPlayerId, true);

            distanceMultiplier = 0.5f;
            requiredRange = 4f;
            cost = 30;

            this.targetPlayerId = targetPlayerId;
        }

        protected override float DistanceToChild (GoapAction child)
        {
            return 0f;
        }

        public override void UpdateTarget()
        {
            if (childs.Count > 0 && cheapestChilds != null && cheapestChilds.Count > 0)
            {
                if (cheapestChilds[0].target != null)
                {
                    target = cheapestChilds[0].target;
                }
            }
            else
            {
                base.UpdateTarget();
            }
        }

        public override void Perform()
        {
            
        }

        public override GoapAction Clone()
        {
            return new ThrowAtPlayerAction(agent, targetPlayerId).SetClone(originalObjectGUID);
        }
    }
}
