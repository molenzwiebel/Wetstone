using ProjectM;

namespace Wetstone.Network.Events;

public class ActivateVBloodAbilityEventArgs : AbstractIncomingEventArgs
{
    public PrefabGUID AbilityGUID { get; }
    public bool PrimarySlot { get; }

    internal ActivateVBloodAbilityEventArgs(PrefabGUID abilityGUID, bool primarySlot)
    {
        AbilityGUID = abilityGUID;
        PrimarySlot = primarySlot;
    }
}