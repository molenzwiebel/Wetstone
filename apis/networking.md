---
layout: default
title: Networking
nav_order: 2
parent: Wetstone APIs
---

Client & Server
{: .label .label-blue }

# Networking APIs

If you have a plugin that runs on both the client and the server, Wetstone offers an easy-to-use API for sending messages between them through V Rising's own network system. Simply register your network type on both the client and the server, and you'll be able to send messages with a single function.

## Graceful Fallbacks

The networking system has been designed to gracefully fail. What this means is that you don't have to worry about crashing anything if you send a message that the other side won't understand. Here's what happens:

- If you have Wetstone installed on your client, but not on the server, and try to send a message to the server: the server will ignore it.
- If you have Wetstone installed on your client and server, but only register the message type on the client: the server will ignore it.
- If you have Wetstone installed on your server, but not on the client, and try to send a message to the client: the client will ignore it.
- If you have Wetstone installed on your client and server, but only register the message type on the server: the client will ignore it.
- If you have Wetstone installed on your client and server, and have the message type registered on both: everything will work fine!

## Network Types

What can you send over the network? You have two options for network types: you can use a _blittable struct_ or you can use your own custom serialization logic.

### Blittable Structs

The easiest way to create a networking type is to use a [_blittable struct_](https://docs.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types). Blittable structs are structs that contain only "simple" types that can be directly copied. Such types include numeric types such as `byte`, `int` and `ulong`, but **not** `bool` or `string` (among others).

Here's an example of a blittable struct:

```csharp
public struct MyMessage
{
    public int MyNumber;
    public MyEnum MyEnum;
    public FixedString128 MyString; // you can use Unity's fixed size strings!
    
    // You can also use arrays of a fixed size, but only if you make the struct "unsafe".
    // public fixed byte myBytes[10];

    // You're not allowed to use any classes or anything of a variable size:
    // public string MyString; // ERR: string is a class
    // public byte[] MyArray; // ERR: MyArray can be of any length
    // public List<int> MyList; // ERR: list is a class
}
```

If your message type is a blittable struct, you don't have to implement any serialization. Wetstone will automatically know how to convert your struct from/to bytes.

Unsure if your struct is blittable? Simply try passing it to any of the `Struct` versions of the Wetstone networking APIs. C# will refuse to compile with a (somewhat helpful) error message if your struct is not blittable.

### Custom Network Types

If you'd like more control over what is sent, or if your message is of a variable size, you'll need to manually implement serialization. To do so, implement the `VNetworkMessage` interface. This will give you direct access to the `NetBufferIn` and `NetBufferOut` that V Rising uses to serialize packets.

Here's an example:

```csharp
public class MyCustomSerializedMessage : VNetworkMessage
{
    public string MyNonBlittableString;

    // You need to implement an empty constructor for when your message is
    // received but not yet serialized.
    public MyCustomSerializedMessage() {}

    // Read your contents from the reader.
    public void Deserialize(NetBufferIn reader)
    {
        MyNonBlittableString = reader.ReadString(Allocator.Temp);
    }

    // Write your contents to the writer.
    public void Serialize(NetBufferOut writer)
    {
        writer.Write(MyNonBlittableString);
    }
}
```

Implementing your own network message gives you more control over how things are sent, but is more fragile. Make sure that your serialization is correct, or you may run into errors or even crashes.

## Listening to Messages

In order to start listening to network messages, you'll need to register your type with the `VNetworkRegistry`. When you register your type, you pass event handlers that will get invoked by Wetstone when a message is received.

If your message is meant to be from client to server, use the `VNetworkRegistry.RegisterServerbound` and `VNetworkRegistry.RegisterServerboundStruct` methods (for a custom type and a blittable struct respectively).

If your message is meant to be from server to client, use the `VNetworkRegistry.RegisterClientbound` and `VNetworkRegistry.RegisterClientboundStruct` methods (for a custom type and a blittable struct respectively).

If your message is meant to be sent both ways, use the `VNetworkRegistry.RegisterBiDirectional` and `VNetworkRegistry.RegisterBiDirectionalStruct` methods (for a custom type and a blittable struct respectively). 

This is how you might register both the blittable and custom network types discussed earlier:

```csharp
// using a blittable struct
VNetworkRegistry.RegisterBiDirectionalStruct<MyMessage>(
    // invoked when the server sends a message to the client
    msg =>
    {
        // msg is a MyMessage instance
        Log.LogInfo($"Just received a MyMessage from the server: {msg.MyString}");
    },

    // invoked when a client sends a message to the server
    // fromCharacter is an instance of FromCharacter, from which
    // you can derive the User and Character entities.
    (fromCharacter, msg) =>
    {
        var user = VWorld.Server.EntityManager.GetComponentData<User>(fromCharacter.User);

        Log.LogInfo($"Just received a MyMessage from user {user.CharacterName}: {msg.MyString}");
    }
);

// using a custom network type
VNetworkRegistry.RegisterBiDirectional<MyCustomSerializedMessage>(
    // invoked when the server sends a message to the client
    msg =>
    {
        // msg is a MyCustomSerializedMessage instance
        Log.LogInfo($"Just received a MyCustomSerializedMessage from the server: {msg.MyNonBlittableString}");
    },

    // invoked when a client sends a message to the server
    // fromCharacter is an instance of FromCharacter, from which
    // you can derive the User and Character entities.
    (fromCharacter, msg) =>
    {
        var user = VWorld.Server.EntityManager.GetComponentData<User>(fromCharacter.User);

        Log.LogInfo($"Just received a MyCustomSerializedMessage from user {user.CharacterName}: {msg.MyNonBlittableString}");
    }
);
```

If your plugin [supports reloading](../getting-started/reloading.html), make sure to unregister your network types when you unload your plugin. Forgetting to do so will lead to an error when you try to register them again:

```csharp
VNetworkRegistry.UnregisterStruct<MyMessage>();
VNetworkRegistry.Unregister<MyCustomSerializedMessage>();
```

## Sending Messages 

To send a message, simply use `VNetwork.SendToClient` or `VNetwork.SendToServer` (or their blittable struct equivalents). Wetstone will take care of scheduling the message to be sent at the nearest opportunity, and to clean up the message after it is sent. If you want to send a message to a client, you'll also need to pass an argument indicating which user should receive the message. Common user identifiers, such as `User` and `FromCharacter`, are supported for this purpose.

Here's how you might send our two events, from the server (we use the [chat hook](../hooks/chat.html) for this example):

```csharp
private static void HandleChatMessage(VChatEvent ev)
{
    if (ev.Message != "!send") return;
    ev.Cancel();

    // to send a blittable struct
    VNetwork.SendToClientStruct<MyMessage>(ev.User, new()
    {
        MyNumber = 42,
        MyString = "Hello, world!"
    });

    // to send a custom network type
    VNetwork.SendToClient(ev.User, new MyCustomSerializedMessage
    {
        MyNonBlittableString = "Hello, world!"
    });
}
```

Sending messages on the client is just as straightforward:

```csharp
if (MyMagicKeybinding.IsPressed)
{
    // to send a blittable struct
    VNetwork.SendToServerStruct<MyMessage>(new()
    {
        MyNumber = 42,
        MyString = "Hello, world!"
    });

    // to send a custom network type
    VNetwork.SendToServer(new MyCustomSerializedMessage
    {
        MyNonBlittableString = "Hello, world!"
    });
}
```

---

For more information on the Networking API, see the [API/VNetwork.cs](https://github.com/molenzwiebel/Wetstone/blob/master/API/VNetwork.cs).