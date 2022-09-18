using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.SmartMergeItemsBetweenInventories;

internal class SmartMergeItemsBetweenInventoriesEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "SmartMergeItemsBetweenInventoriesEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var from = networkEvent.NetBufferIn.ReadNetworkId();
        var to = networkEvent.NetBufferIn.ReadNetworkId();

        var eventArgs = new SmartMergeItemsBetweenInventoriesEventArgs(
            from,
            to
        );
        
        return eventArgs;
    }
}