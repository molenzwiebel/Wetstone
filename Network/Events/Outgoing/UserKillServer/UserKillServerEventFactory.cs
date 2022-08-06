using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using Wetstone.Util;
using ProjectM.Network;
using Wetstone.API;

namespace Wetstone.Network.Events.Outgoing.UserKillServer;

internal class UserKillServerEventFactory : IOutgoingNetworkEventFactory
{
    public string EventName => "UserKillServerEvent";
    public bool Enabled => true;
    
    public AbstractEventArgs Build(OutgoingNetworkEvent networkEvent)
    {
        var networkIdSystem = VWorld.Server.GetExistingSystem<NetworkIdSystem>();

        var userDownedServerEventData = networkEvent.EntityManager.GetComponentData<UserKillServerEvent>(networkEvent.Entity);

        var diedEntity = networkIdSystem._NetworkIdToEntityMap[userDownedServerEventData.Died];
        var killerEntity = networkIdSystem._NetworkIdToEntityMap[userDownedServerEventData.Killer];

        var userKillServer = new UserKillServerEventArgs(
            killerEntity,
            diedEntity
        );
        
        return userKillServer;
    }
}
