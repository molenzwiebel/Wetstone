using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM.Network;
using ProjectM;

namespace Wetstone.Network.Events.Incoming.GiveUpRevive;

internal class GiveDebugEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "GiveDebugEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var prefabguidhash = (int)networkEvent.NetBufferIn.ReadUInt32();
        var ammount = (int)networkEvent.NetBufferIn.ReadUInt32();
        var eventArgs = new GiveDebugEventArgs(ammount, prefabguidhash);

        return eventArgs;
    }
}