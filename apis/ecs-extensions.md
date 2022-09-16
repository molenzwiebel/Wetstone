---
layout: default
title: ECS Extensions
nav_order: 3
parent: Wetstone APIs
---

Client & Server
{: .label .label-blue }

# ECS Extensions

Wetstone offers various classes and extension methods to make working with the V Rising/Unity ECS system easier. If you find yourself repeating the same pattern often, consider making a PR so that it can be added to Wetstone.

## VWorld

The `Wetstone.API.VWorld` class has convenience methods for accessing ECS world instances on both server and client:

- `VWorld.Client`: Returns the currently active client ECS world.
- `VWorld.Server`: Returns the currently active server ECS world.
- `VWorld.Game`: Returns the currently active ECS world, resolving to either `VWorld.Client` or `VWorld.Server` based on whether this is a client or server instance.
- `VWorld.Default`: Returns the default ECS world used at Unity startup. Some global systems, such as the client's `InputSystem`, are registered on this ECS world.
- `VWorld.IsClient`: Returns a boolean indicating whether this is a V Rising client.
- `VWorld.IsServer`: Returns a boolean indicating whether this is a V Rising server.

See [API/VWorld.cs](https://github.com/molenzwiebel/Wetstone/blob/master/API/VWorld.cs) for full documentation.

---

## VExtensions

The `Wetstone.API.VExtensions` class has convenience methods for interacting with the ECS:

- `User.SendSystemMessage` [server-only]: Easily send a system chat message to a client through a `User` instance.
- `Entity.WithComponentData`: Utility method that executes an action argument with a reference to a given entity component. This can be used to directly mutate a component, instead of reading it, updating it, then writing it back:

  ```csharp
  entity.WithComponentData((ref Health health) =>
      health.Value = 10; // directly updates the Health component
  });
  ```

See [API/VExtensions.cs](https://github.com/molenzwiebel/Wetstone/blob/master/API/VExtensions.cs) for full documentation.
