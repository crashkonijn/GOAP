# WorldKeys
`WorldKeys` are used to reference to a specific state in the world. The WorldState is used by the `Planner` to determine which `Action` should be performed.

A `WorlKey` should be referenced by the `WorldSensor` that determines the state for the `WorldKey`.

## Creating a TargetKey using ScriptableObject
Right click on a folder in the editor and select `Create > Goap > WorldKey`.

## Creating a TargetKey using Code
To create a new `WorldKey`, create a new class that inherits from the `WorldKeyBase` class.

### Example

{% code title="IsHungry.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class IsHungry : WorldKeyBase
{
}
```
{% endcode %}
