using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.DeauthAdmin;

internal class DeauthAdminEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "DeauthAdminEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var deauthEvent = new DeauthAdminEventArgs();

        return deauthEvent;
    }
}