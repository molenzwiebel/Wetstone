using Wetstone.Network.Interfaces;
using Wetstone.Network.Models;
using ProjectM.Network;

namespace Wetstone.Network.Events.Incoming.ChatMessage;

internal class ChatMessageEventFactory : IIncomingNetworkEventFactory
{
    public string EventName => "ChatMessageEvent";
    public bool Enabled => true;

    public AbstractEventArgs Build(IncomingNetworkEvent networkEvent)
    {
        var netBufferIn = networkEvent.NetBufferIn;

        var messageType = (ChatMessageType)netBufferIn.ReadByte();
        var messageText = netBufferIn.ReadFixedString512();
        NetworkId? receiverEntity = null;
        
        if (messageType == ChatMessageType.Whisper)
        {
            receiverEntity = netBufferIn.ReadNetworkId();
        }

        var chatMessage = new ChatMessageEventArgs(messageType, messageText, receiverEntity);
        
        return chatMessage;
    }
}