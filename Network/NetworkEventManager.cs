using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wetstone.Network.Events;
using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network;

internal static class NetworkEventManager
{
    private static readonly Dictionary<string, IIncomingNetworkEventFactory> IncomingEventHandlers = new();
    private static readonly Dictionary<string, IOutgoingNetworkEventFactory> OutgoingEventHandlers = new();

    internal static bool HandleEvent(IncomingNetworkEvent networkEvent, out bool cancelled)
    {
        cancelled = false;
        var eventName = NetworkEvents.GetNetworkEventName(networkEvent.EventId);

        

        if (IncomingEventHandlers.TryGetValue(eventName, out var eventFactory) && eventFactory.Enabled)
        {
            var args = eventFactory.Build(networkEvent);
            if (args is AbstractIncomingEventArgs incomingArgs)
            {
                incomingArgs.UserEntity = networkEvent.UserEntity;
            }

            ServerEvent.InvokeEvent(args);
            return true;
        }

        return false;
    }

    internal static bool HandleEvent(OutgoingNetworkEvent networkEvent, out bool cancelled)
    {
        cancelled = false;
        var eventName = NetworkEvents.GetNetworkEventName(networkEvent.EventId);
        if (OutgoingEventHandlers.TryGetValue(eventName, out var eventHandler) && eventHandler.Enabled)
        {
            var args = eventHandler.Build(networkEvent);
            ServerEvent.InvokeEvent(args);
            return true;
        }

        return false;
    }

    internal static void RegisterEvents(Type? type = null)
    {
        type ??= typeof(NetworkEventManager);
        var assembly = Assembly.GetAssembly(type);

        var incomingType = typeof(IIncomingNetworkEventFactory);
        var incomingTypes = assembly.GetTypes().Where(t => incomingType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var incomingEventType in incomingTypes)
        {
            var instance = (IIncomingNetworkEventFactory)Activator.CreateInstance(incomingEventType);
            IncomingEventHandlers[instance.EventName] = instance;
        }

        var outgoingType = typeof(IOutgoingNetworkEventFactory);
        var outgoingTypes = assembly.GetTypes().Where(t => outgoingType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var outgoingEventType in outgoingTypes)
        {
            var instance = (IOutgoingNetworkEventFactory)Activator.CreateInstance(outgoingEventType);
            OutgoingEventHandlers[instance.EventName] = instance;
        }

    }
}