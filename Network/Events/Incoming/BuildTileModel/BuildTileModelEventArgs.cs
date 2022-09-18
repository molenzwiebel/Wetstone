using ProjectM;
using ProjectM.Network;
using ProjectM.Tiles;
using Unity.Mathematics;

namespace Wetstone.Network.Events;

public class BuildTileModelEventArgs : AbstractIncomingEventArgs
{    
    public PrefabGUID PrefabGuid { get; }
    public float3 SpawnPosition { get; }
    public TileRotation SpawnTileRotation { get; }
    public NetworkId TransformedEntity { get; }

    internal BuildTileModelEventArgs(PrefabGUID prefabGuid, float3 spawnPosition, TileRotation spawnTileRotation, NetworkId transformedEntity)
    {
        PrefabGuid = prefabGuid;
        SpawnPosition = spawnPosition;
        SpawnTileRotation = spawnTileRotation;
        TransformedEntity = transformedEntity;
    }
}