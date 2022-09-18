using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.BecomeObserver;

internal class DropInventoryItemEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "DropInventoryItemEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var inventory = networkEvent.NetBufferIn.ReadNetworkId();
        var slot = networkEvent.NetBufferIn.ReadUInt32();

        var dropInventoryItem = new DropInventoryItemEventArgs(inventory, slot);

        return dropInventoryItem;
    }
}