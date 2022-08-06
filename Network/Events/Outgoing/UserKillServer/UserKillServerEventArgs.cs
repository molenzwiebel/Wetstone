using Unity.Entities;

namespace Wetstone.Network.Events;

public class UserKillServerEventArgs : AbstractEventArgs
{
    public Entity Killer;
    public Entity Died;

    internal UserKillServerEventArgs(Entity Killer, Entity Died)
    {
        this.Killer = Killer;
        this.Died = Died;
    }
}