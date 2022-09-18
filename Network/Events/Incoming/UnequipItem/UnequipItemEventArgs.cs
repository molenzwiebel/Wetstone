using ProjectM;
using ProjectM.Network;

namespace Wetstone.Network.Events;

public class UnequipItemEventArgs : AbstractIncomingEventArgs
{
    public EquipmentType EquipmentType;
    public NetworkId ToInventory;
    public uint ToSlotIndex;

    internal UnequipItemEventArgs(EquipmentType equipmentType, NetworkId toInventory, uint toSlotIndex)
    {
        EquipmentType = equipmentType;
        ToInventory = toInventory;
        ToSlotIndex = toSlotIndex;
    }
}