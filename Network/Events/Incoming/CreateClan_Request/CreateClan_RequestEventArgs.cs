namespace Wetstone.Network.Events;

public class CreateClan_RequestEventArgs : AbstractIncomingEventArgs
{    
    public string ClanName { get; }
    public string ClanMotto { get; }

    internal CreateClan_RequestEventArgs(string clanName, string clanMotto)
    {
        ClanName = clanName;
        ClanMotto = clanMotto;
    }
}