using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerGoal : GoapGoal
{
    public KillPlayerGoal(string key, float multiplier = 1) : base(key, multiplier)
    {

    }

    public override void UpdateMultiplier(DataSet data)
    {
        // fancy function that lowers the multiplier if another player kills this AI often
    }
}
