using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;

namespace Wetstone.Network.Events.Incoming.UseDefaultAction;

internal class UseDefaultActionEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "UseDefaultActionEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {

        var action = networkEvent.NetBufferIn.ReadPrefabGUID();
        var exitOnSameForm = networkEvent.NetBufferIn.ReadBoolean();

        var eventArgs = new UseDefaultActionEventArgs(
            action,
            exitOnSameForm
        );
        
        return eventArgs;
    }
}