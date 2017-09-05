using SwordGC.AI.Actions;
using SwordGC.AI.Goap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour {

    Dictionary<GoapAgent, GoapAction> actions = new Dictionary<GoapAgent, GoapAction>();

    private void OnEnable()
    {
        GoapAgent[] agents = FindObjectsOfType<GoapAgent>();
        foreach (GoapAgent agent in agents)
        {
            agent.AddAction(GetAction(agent));
        }
    }

    private void OnDisable()
    {
        GoapAgent[] agents = FindObjectsOfType<GoapAgent>();
        foreach (GoapAgent agent in agents)
        {
            agent.RemoveAction(GetAction(agent));
        }
    }

    private GoapAction GetAction (GoapAgent agent)
    {
        if (!actions.ContainsKey(agent)) actions.Add(agent, new GrabItemAction(agent).SetTarget(gameObject));
        return actions[agent];
    }
}
