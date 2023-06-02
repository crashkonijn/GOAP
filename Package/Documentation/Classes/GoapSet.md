# GoapSet
The `GoapSet` contains all available `Goals` and `Actions` that an agent can use. Different agents can use different `GoapSets`. This allows for different agents to have different `Goals` and `Actions`.

![Screenshot of GoapSetConfig](../images/goap-set.png)

## GoapSet Config
The `GoapSetConfig` is used to configure a `GoapSet`. It contains the following properties:

### goals
The `goals` is a list of `GoalConfigs` that are available to the `Agent`.

### actions
The `actions` is a list of `ActionConfigs` that are available to the `Agent`.

### targetSensors
The `targetSensors` is a list of `TargetSensorConfigs` that are available to the `Agent`.

### worldSensors
The `worldSensors` is a list of `WorldSensorConfigs` that are available to the `Agent`.