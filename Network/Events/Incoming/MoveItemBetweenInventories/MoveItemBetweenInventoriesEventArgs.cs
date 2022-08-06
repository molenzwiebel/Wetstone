using ProjectM.Network;

namespace Wetstone.Network.Events;

public class MoveItemBetweenInventoriesEventArgs : AbstractIncomingEventArgs
{

    public NetworkId FromInventory { get; }
    public uint FromSlot { get; }
    public NetworkId ToInventory { get; }
    public uint ToSlot { get; }
    public ItemTransferMethod TransferMethod { get; }

    internal MoveItemBetweenInventoriesEventArgs(NetworkId fromInventory, uint fromSlot, NetworkId toInventory, uint toSlot, ItemTransferMethod transferMethod)
    {
        FromInventory = fromInventory;
        FromSlot = fromSlot;
        ToInventory = toInventory;
        ToSlot = toSlot;
        TransferMethod = transferMethod;
    }
}