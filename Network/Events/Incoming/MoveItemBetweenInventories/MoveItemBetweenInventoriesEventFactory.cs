using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM.Network;

namespace Wetstone.Network.Events.Incoming.MoveItemBetweenInventories;

internal class MoveItemBetweenInventoriesEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "MoveItemBetweenInventoriesEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var fromInventory = networkEvent.NetBufferIn.ReadNetworkId();
        var fromSlot = networkEvent.NetBufferIn.ReadUInt32();

        // to inventory
        var toInventory = networkEvent.NetBufferIn.ReadNetworkId();
        var toSlot = networkEvent.NetBufferIn.ReadUInt32();

        var transferMethod = (ItemTransferMethod)networkEvent.NetBufferIn.ReadByte();

        var eventArgs = new MoveItemBetweenInventoriesEventArgs(
            fromInventory,
            fromSlot,
            toInventory,
            toSlot,
            transferMethod
        );
        
        return eventArgs;
    }
}