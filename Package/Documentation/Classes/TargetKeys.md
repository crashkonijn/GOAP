# TargetKeys

`TargetKeys` play a pivotal role in the GOAP system by specifying positions or locations within the game environment. These keys help the `Planner` calculate the distance (and added cost) between `Actions` and the precise location an `Agent` needs to reach before executing a particular action.

Each `TargetKey` is associated with a `TargetSensor`. This sensor is responsible for determining and providing the exact position corresponding to the `TargetKey`. In essence, while the `TargetKey` acts as a label or identifier for a location, the `TargetSensor` ensures that this label is mapped to a valid and up-to-date position in the game world.

## Creating a TargetKey

### Using ScriptableObject:

1. In the Unity editor, right-click on a desired folder.
2. Navigate to `Create > Goap > TargetKey` to generate a new `TargetKey`.

### Using Code:

To programmatically create a new `TargetKey`, you'll need to define a new class that inherits from the `TargetKeyBase` class.

#### Example:

{% code title="WanderTarget.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class WanderTarget : TargetKeyBase
{
}
```
{% endcode %}
