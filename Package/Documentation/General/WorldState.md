# WorldState

{% hint style="warning" %}
**Don't use the GOAP WorldState as a source of truth!** In the GOAP system, sensors update the agent's WorldState only when deciding the next action. This means the WorldState can often be outdated. Additionally, using just integers for the WorldState can oversimplify complex situations. For better accuracy and real-time updates, agents should store their data in dedicated MonoBehaviours.
{% endhint %}

## Enhanced GOAP with Integer Values:

In traditional GOAP implementations, the world state is often represented using string keys paired with boolean values. This can lead to redundancy, as multiple keys might be needed to represent related states. By transitioning to integer values, the GOAP system becomes more compact, versatile, and expressive.

### Conditions with Integer Values:

Conditions, which are the prerequisites or requirements for an action to be executed, benefit immensely from this shift:

- **Granular Checks**: Instead of binary checks like "Is the health low?", conditions can now evaluate a spectrum of values, such as:
    - **Health**: `< 30` (Is the health below 30?)
    - **Health**: `>= 70` (Is the health 70 or above?)

- **Comparison Types**: Conditions utilize specific comparison types (like SmallerThan, GreaterThanOrEqual, etc.) to evaluate the integer values of the `WorldKeys`. This allows for diverse condition checks, enabling actions to be contingent on specific thresholds.

- **Absence of "Equals" Comparison**: Notably, there isn't an "Equals" comparison in this system. The primary reason is that "Equals" doesn't indicate direction. In the GOAP system, especially with integer values, understanding the direction of change is crucial. For instance, knowing whether a value needs to increase or decrease to satisfy a condition is essential for planning actions. An "Equals" comparison would be ambiguous in this context, as it wouldn't provide clear guidance on which actions are needed to achieve the desired state.

### Effects with Integer Values:

Effects, which describe the changes an action brings about in the game's state, also gain enhanced expressiveness:

- **Direct Modification**: Instead of toggling boolean states, actions can directly modify integer values. For instance, an action might:
    - **Increase** the "Health" key, representing healing.
    - **Decrease** the "AmmoCount" key, signifying using ammunition.

- **Unified Representation**: Actions that have opposite effects on the same state can be represented using the same `WorldKey`. For example, both healing and taking damage modify the "Health" key, but in opposite directions.

### Benefits:

1. **Reduced Redundancy**: A single integer-based `WorldKey` can represent a range of states, eliminating the need for multiple boolean keys.
2. **Greater Expressiveness**: Conditions and effects can capture a spectrum of values, allowing for nuanced decision-making.
3. **Simplified Logic**: Evaluating conditions and predicting action outcomes become more straightforward with integer values and defined comparison/effect types.
4. **Consistency**: The risk of conflicting or ambiguous world states is reduced, ensuring a more reliable planning process.

### In Summary:

The shift to integer values in the GOAP system offers a more compact and versatile representation of world states, conditions, and effects. By combining integer values with specific comparison and effect types, and by deliberately omitting an "Equals" comparison, the system ensures clarity in action planning. This approach provides AI agents with a broader and more flexible decision-making framework, enabling more informed and context-aware behaviors.
