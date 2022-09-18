namespace Wetstone.Network.Events;

public class DropItemAtSlotEventArgs : AbstractIncomingEventArgs
{
    public int Slot { get; }

    internal DropItemAtSlotEventArgs(int slotIndex)
    {
        Slot = slotIndex;
    }
}