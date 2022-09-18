namespace Wetstone.Network.Events;

public enum EventDirection
{
    /// <summary>
    /// This is an incoming event, it is sent from the client to the server.
    /// </summary>
    ClientServer,
    /// <summary>
    /// This is an outgoing event, it is sent from the server to the client.
    /// </summary>
    ServerClient
}