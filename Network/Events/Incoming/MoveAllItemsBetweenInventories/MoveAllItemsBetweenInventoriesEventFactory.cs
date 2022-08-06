using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.MoveAllItemsBetweenInventories;

internal class MoveAllItemsBetweenInventoriesEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "MoveAllItemsBetweenInventoriesEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var fromInventory = networkEvent.NetBufferIn.ReadNetworkId();
        var toInventory = networkEvent.NetBufferIn.ReadNetworkId();

        var eventArgs = new MoveAllItemsBetweenInventoriesEventArgs(
            fromInventory,
            toInventory
        );

        eventArgs.UserEntity = networkEvent.UserEntity;

        return eventArgs;
    }
}