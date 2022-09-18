using ProjectM.Network;
using Unity.Entities;

namespace Wetstone.Network.Models;

public class OutgoingNetworkEvent
{
    internal OutgoingNetworkEvent(NetworkEventType networkEventType, int eventId, EntityManager entityManager, Entity entity)
    {
        NetworkEventType = networkEventType;
        EventId = eventId;
        EntityManager = entityManager;
        Entity = entity;
    }

    public NetworkEventType NetworkEventType { get; }
    public int EventId { get; }
    public EntityManager EntityManager { get; }
    public Entity Entity { get; }
}
