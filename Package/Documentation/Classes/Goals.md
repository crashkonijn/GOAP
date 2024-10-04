# Goals

In the GOAP system, `Goals` represent the desired outcomes or objectives that an agent aims to achieve. They serve as the starting points for the `Planner`, guiding it in determining the most suitable `Action` to take in order to fulfill a particular `Goal`.

## Goal Config

The `GoalConfig` provides the necessary settings to define and shape a `Goal`. It encompasses several properties:

### 1. Class Type

**Description**: This property specifies the exact type or category of the `Goal`. It helps in identifying and categorizing different goals within the system.

### 2. Conditions

**Description**: Conditions are a set of criteria based on `WorldKeys` that must be met for the `Goal` to be considered achieved. These conditions guide the `Planner` in its decision-making process, helping it select the best `Action` that aligns with the desired outcome.

For instance, if a `Goal` is to "Stay Safe", conditions might include `WorldKeys` like "IsHealthHigh" or "IsInSafeZone".

## Goal Class

The `Goal` class serves as the blueprint for creating specific goals. Key points about the `Goal` class:

- **Inheritance**: Every `Goal` class is derived from the foundational `GoalBase` class. This ensures that all goals share some basic properties and behaviors.

- **Statelessness**: A `Goal` class doesn't maintain any internal state. Its primary role is to provide criteria to the `Planner`, which then uses this information to decide on the most appropriate `Action` to execute.

By understanding and configuring `Goals` appropriately, game developers can guide agents towards desired behaviors, ensuring they act in ways that enhance the gameplay experience.

## Example
{% code title="FixHungerGoal.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

namespace Demos.Goals
{
    public class FixHungerGoal : GoalBase
    {
    }
}
```
{% endcode %}