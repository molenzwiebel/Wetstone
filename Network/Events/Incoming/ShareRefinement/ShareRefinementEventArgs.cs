using ProjectM.Network;

namespace Wetstone.Network.Events;

public class ShareRefinementEventArgs : AbstractIncomingEventArgs
{
    public NetworkId Station { get; }

    internal ShareRefinementEventArgs(NetworkId station)
    {
        Station = station;
    }
}