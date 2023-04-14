# Goals
Goals are used as entry points in the `Planner`. A `Goal` is used to determine the best `Action` that should be performed in order to achieve the `Goal`.

## Goal Config
`GoalConfig` is used to configure a `Goal`. It contains the following properties:

### classType
The `classType` is the type of the `Goal` that should be used.

### conditions
The `conditions` is a list of `WorldKeys` that need to be true or false in order to achieve the `Goal`. Based on these conditions the `Planner` will determine the best `Action` to perform.

## Goal class
A `Goal` class always inherits from the `GoalBase` class. A `Goal` doesn't contain any state, since it is only used by the `Planner` to determine the best `Action` to perform.

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