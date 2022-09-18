using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.Ban;

internal class BanEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "BanEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var unban = (bool)networkEvent.NetBufferIn.ReadBoolean();

        var ban = new BanEventArgs(unban);

        return ban;
    }
}