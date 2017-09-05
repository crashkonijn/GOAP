using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordGC.AI.Actions
{
    public class GrabItemAction : GoapAction
    {

        public GrabItemAction(GoapAgent agent) : base(agent)
        {
            preconditions.Add(Effects.HAS_OBJECT, false);
            effects.Add(Effects.HAS_OBJECT, true);
            
            requiredRange = 4f;
            cost = 20;

            targetString = "Throwable";

            removeWhenTargetless = true;
        }

        public override void Perform()
        {
            
        }

        public override GoapAction Clone()
        {
            return new GrabItemAction(agent).SetTarget(targetString).SetClone(originalObjectGUID);
        }
    }
}
