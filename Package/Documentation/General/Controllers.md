# GOAP Controllers
In the GOAP framework, controllers play a crucial role in managing the behavior of agents within the system. A GOAP Controller is responsible for orchestrating how the GOAP system operates, specifically controlling the execution of sensors and the action resolution process. This allows for the creation of diverse behaviors tailored to the needs of different agents.

A Controller **MUST** be present on the `GoapBehaviour` GameObject in order for the system to work.

## Overview
A GOAP Controller determines the operational flow of the GOAP system, including when and how sensors are run and how actions are resolved. This level of control enables the implementation of various strategies for action planning and execution, leading to more dynamic and adaptable agent behaviors.

## Types of Controllers
The framework introduces three distinct types of controllers, each designed to handle the GOAP system's operations in a unique manner:

### ReactiveController
- **Description**: The ReactiveController operates similarly to the system's behavior in previous version. It activates sensors and the action resolver only when an agent requires a new action. This approach is straightforward and effective for scenarios where agents react to changes in their environment.
- **Usage**: Ideal for agents that operate based on immediate needs or react to changes in the environment.

### ProactiveController
- **Description**: Unlike the ReactiveController, the ProactiveController takes a more forward-looking approach. It periodically runs sensors and the action resolver, even if the agent does not currently need a new action. This proactive behavior can lead to the discovery of more optimal actions or the anticipation of future needs.
- **Usage**: Best suited for agents that benefit from planning ahead or those operating in rapidly changing environments where early action can lead to better outcomes.

#### MayResolve
When using the ProactiveController, sometimes you don't want to re-resolve when certain actions are running. The `IActionRunState` interface implements the `MayResolve` method, which allows you to specify if the resolver may run during this run state.

{% code line_number=true %} 
```csharp
public static readonly ActionRunState Continue = new ContinueActionRunState();
public static readonly ActionRunState ContinueOrResolve = new ContinueOrResolveActionRunState();
public static readonly ActionRunState Stop = new StopActionRunState();
public static readonly ActionRunState Completed = new CompletedActionRunState();
public static ActionRunState Wait(float time, bool mayResolve = false) => new WaitActionRunState(time, mayResolve);
public static ActionRunState WaitThenComplete(float time, bool mayResolve = false) => new WaitThenCompleteActionRunState(time, mayResolve);
public static ActionRunState WaitThenStop(float time, bool mayResolve = false) => new WaitThenStopActionRunState(time, mayResolve);
public static ActionRunState StopAndLog(string message) => new StopAndLog(message);
```
{% endcode %}

### ManualController
- **Description**: The ManualController provides the highest level of control, allowing for the manual execution of sensors and the action resolver. This controller is triggered explicitly by the agent, offering precise control over when the GOAP system is engaged.
- **Usage**: Useful for agents that require direct control over their planning process, such as those in scenarios where timing and precision are critical.
