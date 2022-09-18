using ProjectM.Network;

namespace Wetstone.Network.Events;

public class SmartMergeItemsBetweenInventoriesEventArgs : AbstractIncomingEventArgs
{

    public NetworkId FromInventory;
    public NetworkId ToInventory;
    
    internal SmartMergeItemsBetweenInventoriesEventArgs(NetworkId fromInventory, NetworkId toInventory)
    {
        FromInventory = fromInventory;
        ToInventory = toInventory;
    }
}