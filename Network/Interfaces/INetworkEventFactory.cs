namespace Wetstone.Network.Interfaces;

public interface INetworkEventFactory
{
    string EventName { get; }
    bool Enabled { get; }
}