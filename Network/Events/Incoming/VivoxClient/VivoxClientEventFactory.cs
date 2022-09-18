using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM;

namespace Wetstone.Network.Events.Incoming.VivoxClient;

internal class VivoxClientEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "VivoxClientEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var requestType = (VivoxRequestType)networkEvent.NetBufferIn.ReadByte();

        var eventArgs = new VivoxClientEventArgs(
            requestType
        );
        
        return eventArgs;
    }
}