using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.StartEditTileModel;

internal class StartEditTileModelEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "StartEditTileModelEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var tile = networkEvent.NetBufferIn.ReadNetworkId();

        var eventArgs = new StartEditTileModelEventArgs(tile);
        
        return eventArgs;
    }
}