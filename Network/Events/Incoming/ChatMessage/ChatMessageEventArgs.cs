using ProjectM.Network;
using Unity.Collections;

namespace Wetstone.Network.Events;

public class ChatMessageEventArgs : AbstractIncomingEventArgs
{    
    public ChatMessageType MessageType { get; private set; }
    public FixedString512 MessageText { get; private set; }
    public NetworkId? ReceiverEntity { get; private set; }

    internal ChatMessageEventArgs(ChatMessageType messageType, FixedString512 messageText, NetworkId? receiverEntity)
    {
        MessageType = messageType;
        MessageText = messageText;
        ReceiverEntity = receiverEntity;
    }
}