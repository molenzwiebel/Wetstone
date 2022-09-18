using ProjectM;
using ProjectM.Network;
using Stunlock.Network;

// namespace intentionally omitted

internal static class NetBufferInExtensions
{
    internal static NetworkId ReadNetworkId(this NetBufferIn self)
    {
        var index = self.ReadRangedInteger(0, 0xffffe);
        var generation = self.ReadByte();

        return new NetworkId() {
            Index = index,
            Generation = generation
        };
    }

    internal static PrefabGUID ReadPrefabGUID(this NetBufferIn self)
    {
        return new PrefabGUID((int)self.ReadUInt32());
    }
}