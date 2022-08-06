namespace Wetstone.Network.Events;

public class EquipItemEventArgs : AbstractIncomingEventArgs
{
    public uint SlotIndex { get; }

    internal EquipItemEventArgs(uint slotIndex) {
        SlotIndex = slotIndex;
    }
}