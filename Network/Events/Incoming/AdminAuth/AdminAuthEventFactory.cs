using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.AdminAuth;

internal class AdminAuthEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "AdminAuthEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var adminAuthEvent = new AdminAuthEventArgs();

        return adminAuthEvent;
    }
}