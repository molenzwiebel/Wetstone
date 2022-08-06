using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM.Network;

namespace Wetstone.Network.Events.Incoming.Kill;

internal class KickEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "KickEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var kickEvent = new KickEventArgs();

        return kickEvent;
    }
}