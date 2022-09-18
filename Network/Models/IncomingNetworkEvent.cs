using ProjectM;
using Stunlock.Network;
using Unity.Entities;
using Wetstone.API;

namespace Wetstone.Network.Models;

public class IncomingNetworkEvent
{
    public NetBufferIn NetBufferIn { get; }
    public int EventId { get; }
    public NetworkEvents_Serialize.DeserializeNetworkEventParams EventParams { get; }
    public int FromUserIndex { get; }
    public ServerBootstrapSystem.ServerClient ServerClient { get; }
    public Entity UserEntity { get; }

    internal IncomingNetworkEvent(NetBufferIn netBufferIn, int eventId, NetworkEvents_Serialize.DeserializeNetworkEventParams eventParams)
    {
        NetBufferIn = netBufferIn;
        EventId = eventId;
        EventParams = eventParams;
        FromUserIndex = eventParams.FromUserIndex;

        var serverBootstrap = VWorld.Server.GetExistingSystem<ServerBootstrapSystem>()!;
        ServerClient = serverBootstrap._ApprovedUsersLookup[FromUserIndex];

        UserEntity = ServerClient.UserEntity;
    }

}