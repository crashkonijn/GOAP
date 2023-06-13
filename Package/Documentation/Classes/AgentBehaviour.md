# AgentBehaviour
The `AgentBehaviour` needs to be present on every agent that uses GOAP to determine it's next action. An agent belongs to a `GoapSet`. This set contains the config of all available `Goals` and `Actions`.

The AgentBehaviour contains it's current `Goal` and it's currently active `Action`. The action is determined by the planner based on the current goal and the gamestate (`WorldData`).

## Movement
Each action has a target. This target provides a position that the agent should move to before performing the action. Movement is very game specific and therefore not implemented in this package. The agent events contain a couple of events that can be used to determine when an agent should be moved.

It is also possible to make an action perform while moving, see the `MoveMode` on the `ActionConfig`.

### Distance multiplier
In general the best actions to perform are the fastest way to reach a goal. In that case the cost of an action is probably equal to the time for it to complete. If that's the case than the distance value used in the heuristics should be divided by the move speed of an agent as well.

If you use `SetDistanceMultiplierSpeed(float speed)` to set the (max / average) speed of an agent, the planner will more accurately determine the best action to perform.

## Distance
By default the agent will use the `Vector3.Distance` between the agent and the target as it's distance. You can overwrite this distance by assigning your own implementation of `IAgentDistanceObserver` to the `agent.DistanceObserver` variable. This is useful if you want to use the `navMeshAgent.remainingDistance` for example.

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
        this.GetComponent<AgentBehaviour>.DistanceObserver = this;
    }
    
    public float GetDistance(IMonoAgent agent, ITarget target, IComponentReference reference)
    {
        var distance = this.navMeshAgent.remainingDistance;
        
        // No path
        if (float.IsInfinite(distance))
            return 0f;
        
        return distance;
    }
}
```
{% endcode %}

## Methods

### SetGoal
This method can be used to change the current goal of the agent. The `endAction` parameter determines if the current action should be ended before the new goal is set. If this is set to `false` the current action will be continued until it is finished.
{% code lineNumbers="true" %}
```csharp
public void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase;
public void SetGoal(IGoalBase goal, bool endAction);
```
{% endcode %}

## Determining the Goal
Determining the best `Goal` is very game specific. As such this package does not provide a way to determine the best goal.

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
The `AgentBehaviour` contains a few events that can be used to get notified when the agent changes it's goal or action.

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
