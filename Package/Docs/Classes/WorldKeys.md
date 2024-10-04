# WorldKeys

`WorldKeys` are important in the GOAP system. They point to specific things or situations in the game. The `Planner` uses these keys to decide what `Action` an agent should do next.

Each `WorldKey` is connected to a `WorldSensor`. This sensor checks and gives the current value for its `WorldKey`. So, the `WorldKey` tells us what to look for, and the `WorldSensor` tells us the current value of that thing in the game.

## Creating a WorldKey

### Using ScriptableObject:

1. In the Unity editor, right-click on the folder you want.
2. Go to `Create > Goap > WorldKey` to make a new `WorldKey`.

### Using Code:

You can also make a new `WorldKey` by writing a class that uses the `WorldKeyBase` class.

#### Example:

{% code title="IsHungry.cs" lineNumbers="true" %}
```csharp
using CrashKonijn.Goap.Behaviours;

public class IsHungry : WorldKeyBase
{
}
```
{% endcode %}
