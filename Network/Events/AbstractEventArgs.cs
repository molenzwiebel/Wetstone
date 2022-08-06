namespace Wetstone.Network.Events;

public abstract class AbstractEventArgs
{
    public bool Cancelled { get; set; } = false;
}
