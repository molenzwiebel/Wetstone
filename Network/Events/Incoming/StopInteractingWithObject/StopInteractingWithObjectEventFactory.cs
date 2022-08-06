using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.StopInteractingWithObject;

internal class StopInteractingWithObjectEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "StopInteractingWithObjectEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var target = networkEvent.NetBufferIn.ReadNetworkId();

        var eventArgs = new StopInteractingWithObjectEventArgs(
            target
        );
        
        return eventArgs;
    }
}
