using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.ActivateVBloodAbility;

internal class ActivateVBloodAbilityEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "ActivateVBloodAbilityEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var prefabGUID = networkEvent.NetBufferIn.ReadPrefabGUID();
        var primarySlot = networkEvent.NetBufferIn.ReadBoolean();

        var activateVBloodAbilityEvent = new ActivateVBloodAbilityEventArgs(
            prefabGUID,
            primarySlot
        );

        return activateVBloodAbilityEvent;
    }
}