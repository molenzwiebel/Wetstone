using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using Unity.Mathematics;
using static ProjectM.Network.PlayerTeleportDebugEvent;

namespace Wetstone.Network.Events.Incoming.PlayerTeleportDebug;

internal class PlayerTeleportDebugEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "PlayerTeleportDebugEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {

        var x = networkEvent.NetBufferIn.ReadFloat();
        var y = networkEvent.NetBufferIn.ReadFloat();

        var target = (TeleportTarget)networkEvent.NetBufferIn.ReadUInt32();

        var eventArgs = new PlayerTeleportDebugEventArgs(
            new float2(x, y),
            target
        );
        
        return eventArgs;
    }
}