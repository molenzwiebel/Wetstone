using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.BecomeObserver;

internal class DropItemAtSlotEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "DropItemAtSlotEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var slotIndex = networkEvent.NetBufferIn.ReadInt32();

        var dropItemAtSlot = new DropItemAtSlotEventArgs(slotIndex);

        return dropItemAtSlot;
    }
}