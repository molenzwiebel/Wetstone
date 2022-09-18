using ProjectM.Network;

namespace Wetstone.Network.Events;

public class MoveAllItemsBetweenInventoriesEventArgs : AbstractIncomingEventArgs
{
    public NetworkId FromInventory { get; }
    public NetworkId ToInventory { get; }

    internal MoveAllItemsBetweenInventoriesEventArgs(NetworkId fromInventory, NetworkId toInventory)
    {
        FromInventory = fromInventory;
        ToInventory = toInventory;
    }
}