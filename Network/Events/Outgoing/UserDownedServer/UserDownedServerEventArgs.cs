using Unity.Entities;

namespace Wetstone.Network.Events;

public class UserDownedServerEventArgs : AbstractEventArgs
{
    public Entity Target;
    public Entity Source;

    internal UserDownedServerEventArgs(Entity Target, Entity Source)
    {
        this.Target = Target;
        this.Source = Source;
    }
}