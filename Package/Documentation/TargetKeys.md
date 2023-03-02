# TargetKeys
`TargetKeys` are used to determine the position for a given `TargetKey`. The TargetState is used by the `Planner` to determine distance between `Actions` and the position an `Agent` should move to before performing the action.

A `TargetKey` is ScriptableObject that can be created in the Unity Editor and should be referenced by the `TargetSensor` that determines the position for the `TargetKey`.