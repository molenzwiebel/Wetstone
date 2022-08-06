using ProjectM.Network;
using ProjectM.Tiles;
using Unity.Mathematics;

namespace Wetstone.Network.Events;

public class MoveTileModelEventArgs : AbstractIncomingEventArgs
{

    public NetworkId Tile {get;}

    public float3 Position;

    public TileRotation Rotation;

    internal MoveTileModelEventArgs(NetworkId tile, float3 position, TileRotation rotation)
    {
        Tile = tile;
        this.Position = position;
        this.Rotation = rotation;
    }
}