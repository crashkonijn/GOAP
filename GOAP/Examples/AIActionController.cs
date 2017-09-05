using SwordGC.AI.Actions;
using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionController : GoapAgent {

    public override void Awake()
    {
        base.Awake();

        int testPlayerCount = 3;

        // create goals
        for (int i = 0; i < testPlayerCount; i++)
        {
            if ("Player_"+i == transform.name) continue;

            goals.Add(GoapGoal.Goals.KILL_PLAYER + i, new KillPlayerGoal(GoapGoal.Goals.KILL_PLAYER + i, 1f - (0.5f * i)));
        }

        // create Actions
        for (int i = 1; i < testPlayerCount; i++)
        {
            dataSet.SetData(GoapAction.Effects.KNOCKED_OUT_PLAYER + i, false);

            possibleActions.Add(new KillAction(this, i));
            possibleActions.Add(new KnockoutPlayerAction(this, i));
            possibleActions.Add(new ThrowAtPlayerAction(this, i));
        }
        possibleActions.Add(new GrabItemAction(this));
    }
}
