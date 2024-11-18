# AgentTypes and Capabilities

In the GOAP system, the concepts of `AgentTypes` and `Capabilities` play a crucial role in defining the behavior and abilities of agents within the environment. These concepts allow for a modular and flexible approach to designing agent behaviors, making it easier to create, manage, and extend the functionality of agents.

## AgentTypes

An `AgentType` represents a classification or category of agents within the GOAP system. It acts as a container for a set of `Capabilities` that define what the agent can do. Each agent is associated with an `AgentType`, and all agents of the same type share the same set of capabilities. This means that any goals, actions, and sensors defined within the `AgentType` are available to all agents of that type.

### Key Features of AgentTypes:

- **Shared Behavior**: Since all agents of the same `AgentType` share the same capabilities, they inherently share similar behaviors and abilities. This makes it easier to manage and update the behavior of multiple agents at once.
- **Modularity**: `AgentTypes` allow for the modular design of agent behaviors. By defining different types of agents, developers can easily create diverse ecosystems with varied agent behaviors.
- **Flexibility**: New `AgentTypes` can be created to introduce new kinds of agents into the system, providing flexibility in expanding the behavior space of the application.

## Capabilities

A `Capability` is a collection of goals, actions, and sensors that define a specific set of behaviors an agent can perform. Capabilities are used to modularize and reuse behavior definitions across different `AgentTypes`. Each `Capability` focuses on a particular aspect of behavior, such as navigation, combat, or resource gathering, and can be combined with other capabilities to form complex agent behaviors.

### Key Features of Capabilities:

- **Reusability**: Capabilities can be reused across different `AgentTypes`, allowing for the efficient creation of complex behaviors by combining existing capabilities.
- **Modularity**: By breaking down behaviors into smaller, focused capabilities, the system promotes a modular approach to behavior design. This makes it easier to manage, update, and extend agent behaviors.
- **Flexibility**: Developers can create new capabilities to introduce new behaviors into the system, enhancing the flexibility and adaptability of agents.

### Implementing Capabilities:

Capabilities can be implemented in two ways:

1. **Code**: This approach offers flexibility and allows developers to create custom setup systems and use generics. It is suitable for projects that require a high degree of customization and programmability.
2. **ScriptableObjects**: This approach is more visual and allows developers to set up the system in the Unity Editor. It is ideal for projects that benefit from a more graphical configuration and setup process.