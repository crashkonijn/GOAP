# FAQ

## Why does each action need a target?

Unless you're creating a 0 dimension game, there are actions that take place at a specific position. When there are actions that require positions there a 3 possible solutions for handling the movement.

### 1. There are move actions in the graph.

This approach is extremely inefficient. The graph would become much larger, which makes it much more expensive to calculate the best action. This also requires each specific action to have a specific move action (aka MoveToPlayerAction, MoveToAmmoAction), or you can create a generic action. The generic action however would need to receive data from other actions during the resolving of the graph (aka the previous action would determine the target of the move action).

### 2. Each action is handling movement.

This does make the graph much smaller. However each action would require logic for movement, making them much more complicated. (You'd probably end up with a small FSM in each action; MovingTo, Performing). This also makes it hard to calculate a cost value, including distance between actions. This would again require the previous action in the graph to be provided to each action.

### 3. Each action is performed at a position.

This is the option this project uses. This uses a smaller graph than option 1. This doesn't need to perform movement in an action, keeping them simpler. The graph calculates distance between two actions, adding that cost automatically. Actions don't need to be aware of each other, or their relative position in the graph, making them simpler.

![With move actions](../images/faq\_target\_with\_move\_actions.png) ![Without move actions](../images/faq\_target\_without\_move\_action.png)
