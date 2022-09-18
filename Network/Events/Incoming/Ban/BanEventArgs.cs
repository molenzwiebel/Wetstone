namespace Wetstone.Network.Events.Incoming.Ban;

public class BanEventArgs : AbstractIncomingEventArgs
{
    public bool Unban { get; }

    internal BanEventArgs(bool unban)
    {
        Unban = unban;
    }
}