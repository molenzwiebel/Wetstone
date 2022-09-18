using ProjectM;
using ProjectM.Network;

namespace Wetstone.Network.Events;

public class StartCraftItemEventArgs : AbstractIncomingEventArgs
{
    public NetworkId Workstation;
    public PrefabGUID RecipeId;
        
    internal StartCraftItemEventArgs(NetworkId workstation, PrefabGUID recipeId)
    {
        Workstation = workstation;
        RecipeId = recipeId;
    }

}