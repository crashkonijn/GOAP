# Examples > Simple

The simple example use `ScriptableObject` as the configuration method. This is the easiest way to get started with GOAP. The demo scene can be found in `Demos/Simple/Scenes/SimpleDemo.unity`.

Each agent has 2 separate goals: `WanderGoal` and `FixHungerGoal`. The `WanderGoal` will make the agent wander around the scene. The `FixHungerGoal` will make the agent eat apples. The agent will only eat apples if it is hungry. The agent will only wander if it is not hungry.

Goals:
* WanderGoal
* FixHungerGoal

Actions:
* WanderAction
* EatAppleActions
* PickupAppleAction
* PluckAppleAction

## Rules
If the agent has a `hunger > 80`, it will switch to the `FixHungerGoal`. If the agent has a `hunger < 20`, it will switch to the `WanderGoal`.

![Simple Demo Graph](../images/demo_simple_graph.png)