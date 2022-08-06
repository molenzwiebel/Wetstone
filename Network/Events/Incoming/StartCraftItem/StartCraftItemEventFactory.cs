using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM.Network;

namespace Wetstone.Network.Events.Incoming.StartCraftItem;

internal class StartCraftItemEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "StartCraftItemEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var workstation = networkEvent.NetBufferIn.ReadNetworkId();
        var recipeId = networkEvent.NetBufferIn.ReadPrefabGUID();
        
        var eventArgs = new StartCraftItemEventArgs(
            workstation,
            recipeId
        );
        
        return eventArgs;
    }
}