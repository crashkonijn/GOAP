# Conditions and Effects

## Conditions
Conditions are used to determine what game states (WorldKey) are required for an `Action` to be performed. Based on the requirements of a `Condition` it is matched with `Effects` of other `Actions`. This is used to build the `Graph` that is used by the GOAP Planner.

A condition exists of 3 values:
* `key` - The `WorldKey` that is being checked.
* `comparison` - The comparison that is used to check the `WorldKey` value.
* `value` - The value that is used in the comparison.

## Effects
Effects are used to determine what game states (WorldKey) are changed by an `Action`. Effects can be `positive` (make a value higher) or `negative` (lower a value).

An effect exists of 2 values:
* `key` - The `WorldKey` that is being changed.
* `increase` - Whether the value is increased or decreased.

## Matching Conditions and Effects

The comparison enum consists of the following values:

```csharp
public enum Comparison
{
    SmallerThan,
    SmallerThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual
}
```

`SmallerThan` and `SmallerThanOrEqual` are matched with `negative` effects and `GreaterThan` and `GreaterThanOrEqual` are matched with `positive` effects.