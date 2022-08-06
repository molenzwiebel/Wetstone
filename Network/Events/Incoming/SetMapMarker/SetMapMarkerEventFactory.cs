using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.SetMapMarker;

internal class SetMapMarkerEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "SetMapMarkerEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var x = networkEvent.NetBufferIn.ReadFloat();
        var y = networkEvent.NetBufferIn.ReadFloat();

        var mapMarker = new SetMapMarkerEventArgs(new Unity.Mathematics.float2(x, y));

        return mapMarker;
    }
}