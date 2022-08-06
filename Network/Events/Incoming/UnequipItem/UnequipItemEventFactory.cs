using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM;

namespace Wetstone.Network.Events.Incoming.UnequipItem;

internal class UnequipItemEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "UnequipItemEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var equipmentType = (EquipmentType)networkEvent.NetBufferIn.ReadByte();
        var toInventory = networkEvent.NetBufferIn.ReadNetworkId();
        var toSlotIndex = networkEvent.NetBufferIn.ReadUInt32();

        var eventArgs = new UnequipItemEventArgs(
            equipmentType,
            toInventory,
            toSlotIndex
        );

        return eventArgs;
    }
}