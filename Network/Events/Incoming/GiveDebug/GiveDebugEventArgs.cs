using ProjectM;

namespace Wetstone.Network.Events;

public class GiveDebugEventArgs : AbstractIncomingEventArgs
{
    public int Ammount { get; }

    public int PrefabGuidHash { get; }

    internal GiveDebugEventArgs(int ammount, int prefabguidhash) {
        
        Ammount = ammount;
        PrefabGuidHash = prefabguidhash;    

    }
}