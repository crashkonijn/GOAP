# Data Injection in GOAP

**Data Injection** is a design pattern where an external system provides runtime data to another object or module. In the context of the Goal-Oriented Action Planning (GOAP) system, injection is used to provide specific scene data or dependencies to the core classes (`Goals`, `Actions`, and `Sensors`) managed by the GOAP system.

## Why is Data Injection Needed?

1. **Decoupling**: GOAP classes are designed to be generic and reusable. By injecting specific data or dependencies from the scene or other systems, you can customize their behavior without modifying their core logic. This separation ensures that the GOAP system remains modular and maintainable.

2. **Flexibility**: Different scenes or game scenarios might require different data or behaviors. Injection allows you to provide the necessary context to the GOAP classes, enabling them to adapt to various game situations.

3. **Integration with Third-party Libraries**: By using injection, you can easily integrate third-party libraries or systems with the GOAP framework. For instance, the documentation mentions integrating Zenject, a popular dependency injection framework in Unity.

## How Does It Work?

1. **Creating an Injector**: You create a `MonoBehaviour` class that implements the `IGoapInjector` interface. This class will contain methods that are called right after each GOAP class (`Goal`, `Action`, or `Sensor`) is instantiated. Within these methods, you can provide the necessary data or dependencies to the GOAP classes.

2. **Connecting the Injector**: To let the GOAP system know about your custom injector, you create a class extending `GoapConfigInitializerBase` and bind it to the `GoapRunnerBehaviour` component in the scene. This ensures that your injector is used instead of the default one.

## Example
In the provided example, the `GoapInjector` class is an injector that provides specific scene data (`ItemFactory`, `ItemCollection`, and `InstanceHandler`). The `CreateItemAction` class is an example of a GOAP action that requires this scene data. The method of signaling the injector to provide the necessary data can vary, and the `IInjectable` interface is just one possible approach.

{% code title="GoapInjector.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Interfaces;

public class GoapInjector : MonoBehaviour, IGoapInjector
{
    public ItemFactory itemFactory;
    public ItemCollection itemCollection;
    public InstanceHandler instanceHandler;
    
    public void Inject(IActionBase action)
    {
        if (action is IInjectable injectable)
            injectable.Inject(this);
    }

    public void Inject(IGoalBase goal)
    {
    }

    public void Inject(IWorldSensor worldSensor)
    {
    }

    public void Inject(ITargetSensor targetSensor)
    {
    }
}
```
{% endcode %}

{% code title="CreateItemAction.cs" lang="csharp" %}
```csharp
namespace Demos.Complex.Actions
{
    public class CreateItemAction<TCreatable> : ActionBase<CreateItemAction<TCreatable>.Data>, IInjectable
        where TCreatable : ItemBase, ICreatable
    {
        private ItemFactory itemFactory;
        private InstanceHandler instanceHandler;

        public void Inject(GoapInjector injector)
        {
            this.itemFactory = injector.itemFactory;
            this.instanceHandler = injector.instanceHandler;
        }
        
        // rest of class
    }
}
```
{% endcode %}

## Connecting the injector
In order to let the GOAP know you'd like to overwrite one of it's core settings, the `IGoapInjector` in this case you need to create a class that extends `GoapConfigInitializerBase`.

Add the script to the scene and bind it to the `GoapConfigInitializer` property of the `GoapRunnerBehaviour` component.

![Goap Config Initializer](../images/goap_config_initializer.png)

### Example

{% code title="GoapConfigInitializer.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;

namespace Demos.Complex.Goap
{
    public class GoapConfigInitializer : GoapConfigInitializerBase
    {
        public override void InitConfig(GoapConfig config)
        {
            config.GoapInjector = this.GetComponent<GoapInjector>();
        }
    }
}
```
{% endcode %}

## Zenject
It's very easy to use Zenject with the GOAP. The GOAP has a built-in injector that can be used to inject Zenject dependencies into the GOAP classes.

{% code title="ZenjectGoapInjector.cs" lang="csharp" %}
```csharp
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using Zenject;

public class ZenjectGoapInjector : MonoBehaviour, IGoapInjector
{
    private DiContainer container;

    [Inject]
    private void Construct(DiContainer container)
    {
        this.container = container;
    }
    
    public void Inject(IActionBase action)
    {
        this.container.Inject(action);
    }

    public void Inject(IGoalBase goal)
    {
        this.container.Inject(goal);
    }

    public void Inject(IWorldSensor worldSensor)
    {
        this.container.Inject(worldSensor);
    }

    public void Inject(ITargetSensor targetSensor)
    {
        this.container.Inject(targetSensor);
    }
}
```
{% endcode %}
