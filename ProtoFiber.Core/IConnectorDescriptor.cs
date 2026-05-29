namespace ProtoFiber.Core;

public interface IConnectorDescriptor
{

    protected byte[]? Connections { get; }

    bool IsConnected(int opposite) => IsConnected((byte)opposite);
    bool IsConnected(byte opposite) => Connections is null || Connections.Contains(opposite);

}
