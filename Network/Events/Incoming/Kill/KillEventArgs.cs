using ProjectM.Network;

namespace Wetstone.Network.Events;

public class KillEventArgs : AbstractIncomingEventArgs
{
    public KillWho Who { get; }
    public KillWhoFilter Filter { get; }
    public NetworkId NetworkId { get; }

    internal KillEventArgs(KillWho who, KillWhoFilter filter, NetworkId networkId)
    {
        Who = who;
        Filter = filter;
        NetworkId = networkId;
    }
}