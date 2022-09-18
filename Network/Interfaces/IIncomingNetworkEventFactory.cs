using Wetstone.Network.Events;
using Wetstone.Network.Models;

namespace Wetstone.Network.Interfaces;

public interface IIncomingNetworkEventFactory : INetworkEventFactory
{
    AbstractEventArgs Build(IncomingNetworkEvent networkFactory);
}