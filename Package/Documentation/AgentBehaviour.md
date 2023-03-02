# AgentBehaviour
The `AgentBehaviour` needs to be present on every agent that uses GOAP to determine it's next action. An agent belongs to a `GoapSet`. This set contains the config of all available `Goals` and `Actions`.

The AgentBehaviour contains it's current `Goal` and it's currently active `Action`. The action is determined by the planner based on the current goal and the gamestate (`WorldData`).

## Movement
Each action has a target. This target provides a position that the agent should move to before performing the action. Movement is very game specific and therefore not implemented in this package. Each agent should contain a MonoBehaviour that implements `IAgentMover`. This interface contains a single method `Move(ITarget target)` that is called by the `AgentBehaviour` when it needs to move to a target. This is called in the `Update` method of the `AgentBehaviour`.

## Methods

### SetGoal
This method can be used to change the current goal of the agent. The `endAction` parameter determines if the current action should be ended before the new goal is set. If this is set to `false` the current action will be continued until it is finished.
```c#
public void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase;
public void SetGoal(IGoalBase goal, bool endAction);
```

## Determining the Goal
Determining the best `Goal` is very game specific. As such this package does not provide a way to determine the best goal.

### Example
This is an example of how to determine the best goal. In this example the agent will wander around until it's hunger is above 80. When it's hunger is above 80 it will try to fix it's hunger. When it's hunger is below 20 it will wander around again.

```c#
using System;
using CrashKonijn.Goap.Behaviours;
using Demos.Goals;
using UnityEngine;

namespace Demos.Behaviours
{
    public class AgentBrain : MonoBehaviour
    {
        private AgentBehaviour agent;
        private HungerBehaviour hunger;

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
            this.hunger = this.GetComponent<HungerBehaviour>();
        }

        private void Start()
        {
            this.agent.SetGoal<WanderGoal>(false);
        }

        private void FixedUpdate()
        {
            if (this.hunger.hunger > 80)
                this.agent.SetGoal<FixHungerGoal>(false);
            
            if (this.hunger.hunger < 20)
                this.agent.SetGoal<WanderGoal>(true);
        }
    }
}
```