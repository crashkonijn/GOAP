# WorldKeys
`WorldKeys` are used to reference to a specific state in the world. The WorldState is used by the `Planner` to determine which `Action` should be performed.

A `WorlKey` is ScriptableObject that can be created in the Unity Editor and should be referenced by the `WorldSensor` that determines the state for the `WorldKey`.