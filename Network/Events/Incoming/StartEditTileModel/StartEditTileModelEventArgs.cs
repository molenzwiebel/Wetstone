using ProjectM.Network;

namespace Wetstone.Network.Events;

public class StartEditTileModelEventArgs : AbstractIncomingEventArgs
{
    public NetworkId Tile { get; }

    internal StartEditTileModelEventArgs(NetworkId tile)
    {
        Tile = tile;
    }
}