using ProjectM;

namespace Wetstone.Network.Events;

public class EnterShapeshiftEventArgs : AbstractIncomingEventArgs
{
    public PrefabGUID Shapeshift;
    public bool ExitOnSameForm;

    internal EnterShapeshiftEventArgs(PrefabGUID shapeshift, bool exitOnSameForm)
    {
        Shapeshift = shapeshift;
        ExitOnSameForm = exitOnSameForm;
    }
}