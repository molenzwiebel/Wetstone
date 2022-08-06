using Wetstone.Network.Events;
using Wetstone.Network.Models;

namespace Wetstone.Network.Interfaces;

public interface IOutgoingNetworkEventFactory : INetworkEventFactory
{
    AbstractEventArgs Build(OutgoingNetworkEvent networkFactory);
}