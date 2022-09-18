using ProjectM.Network;

namespace Wetstone.Network.Events;

public class SortAllItemsEventArgs : AbstractIncomingEventArgs
{

    public NetworkId Inventory {get;}

    internal SortAllItemsEventArgs(NetworkId inventory) {
        this.Inventory = inventory;
    }
    
}