using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.ChatMessage;

internal class CreateClan_RequestEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "CreateClan_Request";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var netBufferIn = networkEvent.NetBufferIn;

        var clanName = netBufferIn.ReadFixedString64();
        var clanMotto = netBufferIn.ReadFixedString64();

        var createClan_Request = new CreateClan_RequestEventArgs(clanName.ToString(), clanMotto.ToString());
        
        return createClan_Request;
    }
}