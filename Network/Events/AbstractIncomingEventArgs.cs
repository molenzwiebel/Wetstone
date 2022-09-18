using Unity.Entities;

namespace Wetstone.Network.Events;

public abstract class AbstractIncomingEventArgs : AbstractEventArgs
{
    public Entity UserEntity { get; internal set; }
}