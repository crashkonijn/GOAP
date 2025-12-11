# Upgrade guid v3.0 to v3.1

## Breaking changes

### AgentTypeBuilder
Instead of manually creating an `AgentTypeBuilder` instance, you should now use the `CreateBuilder` method on the `AgentTypeFactoryBase` class.

```csharp
// Old way
var builder = new AgentTypeBuilder(SetIds.Smith);

// New way
var builder = this.CreateBuilder(SetIds.Smith);
```

### IGoapInjector
The `IGoapInjector` interface has been expanded with two new methods:

```csharp
public interface IGoapInjector
{
    void Inject(IAction action);
    void Inject(IGoal goal);
    void Inject(ISensor sensor);
    // New methods
    void Inject(IAgentTypeFactory factory);
    void Inject(ICapabilityFactory factory);
}
```

## New Features

### You can now make dynamic conditions (code only)!

```csharp
builder.AddAction<EatAction>()
    .AddCondition<Hunger, LowHunger>(Comparison.GreaterThanOrEqual);
```

### AgenTypeFactory and CapabilityFactory can now be injected
You can now inject into `AgentTypeFactoryBase` and `CapabilityFactoryBase` classes, similar to other actions, goals and sensors.

### You can now manually call AgentBehaviour.Run() with custom deltaTimes
This allows you to determine the rate at which agents are updated, for example agents further away from the player.

Make sure to set `AgentBehaviour.RunInUnityUpdate` to false before calling `Run` yourself!

```csharp
agentBehaviour.RunInUnityUpdate = false;
agentBehaviour.Run(1f);

// In actions
public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
{
    // Make sure to use the deltaTime provided in the context!
    var deltaTime = context.DeltaTime;
}
```