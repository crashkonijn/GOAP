# AgentBehaviour and GoapActionProvider

## Overview
In version 3 (v3) of the GOAP framework, significant architectural changes have been introduced to enhance the flexibility and functionality of agents and their actions. One of the key changes is the separation of concerns between the AgentBehaviour and the GoapActionProvider. This document aims to explain the functionalities of both components and their relationship.

## AgentBehaviour
The AgentBehaviour component is responsible for the execution of actions. It acts as the executor that takes an action provided by the GoapActionProvider and performs it. The AgentBehaviour is designed to be agnostic of the decision-making process, focusing solely on action execution.

### Key Responsibilities
- **Action Execution**: Takes an action from the GoapActionProvider and executes it.
- **Event Handling**: Can subscribe to and trigger events related to action execution, such as OnActionStart, OnActionEnd, and OnActionComplete.

### Relationship with GoapActionProvider
The AgentBehaviour does not directly interact with the GoapActionProvider for decision-making. It only knows about a simple IActionProvider interface, which abstracts the source of actions.

## GoapActionProvider
The GoapActionProvider is a new addition in v3, designed to handle the decision-making process for the agent. It decides which actions to perform and when to perform them, based on the goals set and the current state of the world.

### Key Responsibilities
- **Action Decision**: Decides which actions are suitable for execution based on the current goals and state.
- **Goal Management**: Manages goals for the agent, including setting new goals and prioritizing between multiple goals.
- **GOAP Methods**: Contains all GOAP-related methods that were previously part of the AgentBehaviour, such as goal setting and action planning.

### Relationship with AgentBehaviour
The GoapActionProvider does not know about the AgentBehaviour. It interacts with the agent through a simple IActionReceiver interface, focusing on the decision-making process without concerning itself with how actions are executed.

## Interaction Example

Here is a simplified example of how AgentBehaviour and GoapActionProvider interact within the system:

{% code lineNumbers="true" %}
```csharp
// Getting references to both components
var agent = this.GetComponent<AgentBehaviour>();
var provider = this.GetComponent<GoapActionProvider>();

// Connecting the AgentBehaviour to the GoapActionProvider
agent.ActionProvider = provider;

// Setting a goal through the GoapActionProvider
provider.RequestGoal<FixHungerGoal>();

// Accessing the current action and goal for debugging
Debug.Log(agent.ActionState.Action);
Debug.Log(provider.CurrentPlan.Goal);

// Subscribing to events
agent.Events.OnTargetInRange += this.OnTargetInRange;
agent.Events.OnTargetChanged += this.OnTargetChanged;
provider.Events.OnGoalStart += this.OnGoalStart;
```
{% endcode %}

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