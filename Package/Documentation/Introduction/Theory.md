# What is Goap?

Goal Oriented Action Planning (GOAP) is a technique commonly used in game AI to create agents that can autonomously determine their actions and achieve specific goals within the game environment. GOAP can be used to create complex and adaptive behavior for non-player characters (NPCs) in a game.

The basic idea of GOAP is to break down a complex task or goal into a series of smaller, simpler actions that an agent can perform. These actions are then organized into a plan or sequence of actions that will lead the agent towards the desired goal.

GOAP begins with the agent evaluating its current state and the desired goal state. The agent then searches through a library of available actions to find a series of actions that will transform the current state into the desired goal state. Each action is associated with a set of preconditions that must be met in order for the action to be executed. For example, an action to pick up a key might have a precondition that the key is in the same room as the agent.

Once a plan has been created, the agent executes the first action in the plan. As the agent completes each action, it re-evaluates its state and the remaining actions in the plan to ensure that it is still on track towards its goal. If the agent encounters an obstacle or a change in the game environment, it can dynamically adjust its plan to find a new sequence of actions that will still lead to the desired goal.

GOAP is particularly useful in games with complex environments and multiple goals, as it allows NPCs to adapt to changing situations and make decisions based on their current state and the desired outcome. It can also be used to create NPCs with different personalities or behavior patterns by adjusting the weighting of different actions or goals in their decision-making process.

Overall, GOAP is a powerful technique for creating intelligent and adaptive agents in games that can perform complex tasks and achieve specific goals within the game world.
