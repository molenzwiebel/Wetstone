using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM.Network;

namespace Wetstone.Network.Events.Incoming.GiveUpRevive;

internal class GiveUpReviveEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "GiveUpReviveEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var eventArgs = new GiveUpReviveEventArgs();
        
        return eventArgs;
    }
}