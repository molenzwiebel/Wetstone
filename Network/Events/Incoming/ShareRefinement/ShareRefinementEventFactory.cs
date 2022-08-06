using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.ShareRefinement;

internal class ShareRefinementEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "ShareRefinementEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var station = networkEvent.NetBufferIn.ReadNetworkId();

        var eventArgs = new ShareRefinementEventArgs(
            station
        );
        
        return eventArgs;
    }
}