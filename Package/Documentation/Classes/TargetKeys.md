# TargetKeys
`TargetKeys` are used to determine the position for a given `TargetKey`. The TargetState is used by the `Planner` to determine distance between `Actions` and the position an `Agent` should move to before performing the action.

A `TargetKey` should be referenced by the `TargetSensor` that determines the position for the `TargetKey`.

## Creating a TargetKey using ScriptableObject
Right click on a folder in the editor and select `Create > Goap > TargetKey`.

## Creating a TargetKey using Code
To create a new `TargetKey`, create a new class that inherits from the `TargetKeyBase` class.

### Example

{% code title="WanderTarget.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class WanderTarget : TargetKeyBase
{
}
```
{% endcode %}
