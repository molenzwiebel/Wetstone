namespace Wetstone.Network.Events;

public class BecomeObserverEventArgs : AbstractIncomingEventArgs
{    
    public int Mode {get;}

    internal BecomeObserverEventArgs(int mode)
    {
        Mode = mode;
    }
}