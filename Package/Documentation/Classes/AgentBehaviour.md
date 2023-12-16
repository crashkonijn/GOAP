# AgentBehaviour

The `AgentBehaviour` is a crucial component that must be attached to every agent leveraging the GOAP system to decide its subsequent actions. It links an agent to a specific `GoapSet`, which encompasses the configuration of all potential `Goals` and `Actions` the agent can undertake.

## Overview

- **Current Goal**: The objective the agent is currently trying to achieve.
- **Active Action**: The action the agent is currently executing to meet its goal.
- **WorldData**: Represents the game's current state, which the planner uses to decide the best action for the agent.

## Movement

Actions often have associated targets, indicating a position the agent should reach before executing the action. Since movement mechanics can vary based on the game's design, this package doesn't prescribe a specific movement implementation. However, it provides events to help developers determine when an agent should move.

### MoveMode

Some actions might need the agent to perform tasks while moving. The `MoveMode` in the `ActionConfig` allows for such configurations.

### Distance Multiplier

The primary objective of actions is to achieve goals swiftly. If the action's cost equates to its completion time, then the heuristic's distance value should be divided by the agent's movement speed. Using `SetDistanceMultiplierSpeed(float speed)` sets the agent's (max/average) speed, enabling the planner to more precisely ascertain the optimal action.

### Custom Distance Calculation

By default, the agent calculates distance using `Vector3.Distance`. However, for more complex scenarios, like using a nav mesh, you can override this by assigning your custom `IAgentDistanceObserver` to the `agent.DistanceObserver`.

### Example

{% code title="NavMeshDistanceObserver.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDistanceObserver : MonoBehaviour, IAgentDistanceObserver
{
    private NavMeshAgent navMeshAgent;
    
    private void Awake()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.GetComponent<AgentBehaviour>().DistanceObserver = this;
    }
    
    public float GetDistance(IMonoAgent agent, ITarget target, IComponentReference reference)
    {
        var distance = this.navMeshAgent.remainingDistance;
        
        // No path
        if (float.IsInfinity(distance))
            return 0f;
        
        return distance;
    }
}
```
{% endcode %}

## Methods

### SetGoal
This method allows for the modification of the agent's current goal. The `endAction` parameter decides if the ongoing action should terminate before setting the new goal.

{% code lineNumbers="true" %}
```csharp
public void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase;
public void SetGoal(IGoalBase goal, bool endAction);
```
{% endcode %}

## Determining the Goal
Choosing the best `Goal` is game-specific, and this package doesn't dictate a method. However, an example is provided below to illustrate how one might determine a goal based on an agent's hunger level.

### Example
This is an example of how to determine the best goal. In this example the agent will wander around until it's hunger is above 80. When it's hunger is above 80 it will try to fix it's hunger. When it's hunger is below 20 it will wander around again.

{% code title="AgentBrain.cs" lineNumbers="true" %}
```csharp
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
{% endcode %}

## Events
`AgentBehaviour` offers several events that notify developers when the agent alters its goal or action. These events can be instrumental in managing agent behaviors and responses.

### Example

{% code title="EventExample.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class EventExample : MonoBehaviour
    {
        private AgentBehaviour agent;

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            this.agent.Events.OnActionStart += this.OnActionStart;
            this.agent.Events.OnActionStop += this.OnActionStop;
            this.agent.Events.OnGoalStart += this.OnGoalStart;
            this.agent.Events.OnNoActionFound += this.OnNoActionFound;
            this.agent.Events.OnGoalCompleted += this.OnGoalCompleted;
        }

        private void OnDisable()
        {
            this.agent.Events.OnActionStart -= this.OnActionStart;
            this.agent.Events.OnActionStop -= this.OnActionStop;
            this.agent.Events.OnGoalStart -= this.OnGoalStart;
            this.agent.Events.OnNoActionFound -= this.OnNoActionFound;
            this.agent.Events.OnGoalCompleted -= this.OnGoalCompleted;
        }

        private void OnActionStart(IActionBase action)
        {
            // Gets called when an action is started
        }

        private void OnActionStop(IActionBase action)
        {
            // Gets called when an action is stopped
            // This can be used to check for a new goal
        }

        private void OnGoalStart(IGoalBase goal)
        {
            // Gets called when a goal is started
        }

        private void OnGoalCompleted(IGoalBase goal)
        {
            // Gets called when a goal is completed
        }

        private void OnNoActionFound(IGoalBase goal)
        {
            // Gets called when no action is found for a goal
            // This can be used to add a backup goal for example
        }
    }
}
```
{% endcode %}

{% code title="AgentMoveBehaviour.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

public class AgentMoveBehaviour : MonoBehaviour
{
    private AgentBehaviour agent;
    private ITarget currentTarget;
    private bool shouldMove;

    private void Awake()
    {
        this.agent = this.GetComponent<AgentBehaviour>();
    }

    private void OnEnable()
    {
        this.agent.Events.OnTargetInRange += this.OnTargetInRange;
        this.agent.Events.OnTargetChanged += this.OnTargetChanged;
        this.agent.Events.OnTargetOutOfRange += this.OnTargetOutOfRange;
    }

    private void OnDisable()
    {
        this.agent.Events.OnTargetInRange -= this.OnTargetInRange;
        this.agent.Events.OnTargetChanged -= this.OnTargetChanged;
        this.agent.Events.OnTargetOutOfRange -= this.OnTargetOutOfRange;
    }

    private void OnTargetInRange(ITarget target)
    {
        this.shouldMove = false;
    }

    private void OnTargetChanged(ITarget target, bool inRange)
    {
        this.currentTarget = target;
        this.shouldMove = !inRange;
    }

    private void OnTargetOutOfRange(ITarget target)
    {
        this.shouldMove = true;
    }

    public void Update()
    {
        if (!this.shouldMove)
            return;
        
        if (this.currentTarget == null)
            return;
        
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.currentTarget.Position.x, this.transform.position.y, this.currentTarget.Position.z), Time.deltaTime);
    }
}
```
{% endcode %}
