using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using Wetstone.Util;
using ProjectM.Network;
using Wetstone.API;

namespace Wetstone.Network.Events.Outgoing.UserDownedServer
{
    internal class UserDownedServerEventFactory : IOutgoingNetworkEventFactory
    {
        public string EventName => "UserDownedServerEvent";
        public bool Enabled => true;
        
        public AbstractEventArgs Build(OutgoingNetworkEvent networkEvent)
        {
            var networkIdSystem = VWorld.Server.GetExistingSystem<NetworkIdSystem>();

            var userDownedServerEventData = networkEvent.EntityManager.GetComponentData<UserDownedServerEvent>(networkEvent.Entity);

            var targetEntity = networkIdSystem._NetworkIdToEntityMap[userDownedServerEventData.Target];
            var sourceEntity = networkIdSystem._NetworkIdToEntityMap[userDownedServerEventData.Source];


            // This is an interesting one.
            // Target is always a player, which is obvious.
            // Source can have PlayerCharacter, if it does not, it's most likely an NPC, not sure which component to check for.
            // Source can also be equal to Target, when the player dies from the sun, eg, suicide.
            // Which is a literal `if (e.Target == e.Source)` check.

            // My goal (for this project) is to make these events as user-friendly as possible,
            // where the user (developer) doesn't need to do a bazillion GetComponentData's to get the information available on the entities
            // If we can automate that somehow, by adding all the useful bits of information to the EventArgs (not just for this one)
            // That'd be amazing.


            // var targetPlayerCharacter = networkEvent.EntityManager.GetComponentData<PlayerCharacter>(targetEntity);
            // var sourcePlayerCharacter = networkEvent.EntityManager.GetComponentData<PlayerCharacter>(sourceEntity);

            var userDownedServer = new UserDownedServerEventArgs(
                targetEntity,
                sourceEntity
            );

            return userDownedServer;
        }
    }
}
