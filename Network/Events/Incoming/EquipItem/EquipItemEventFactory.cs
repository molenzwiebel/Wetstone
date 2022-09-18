using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.EquipItem;

internal class EquipItemEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "EquipItemEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var slotIndex = networkEvent.NetBufferIn.ReadUInt32();

        var eventArgs = new EquipItemEventArgs(
            slotIndex
        );
        
        return eventArgs;
    }
}