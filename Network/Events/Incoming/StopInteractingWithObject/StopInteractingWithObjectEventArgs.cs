using ProjectM.Network;

namespace Wetstone.Network.Events;

public class StopInteractingWithObjectEventArgs : AbstractIncomingEventArgs
{
    public NetworkId Target { get; }

    internal StopInteractingWithObjectEventArgs(NetworkId target)
    {
        Target = target;
    }
}