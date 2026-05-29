namespace ProtoFiber.Core;

public enum ConnectorDirection : byte
{
    Invalid = 0,
    /// <summary>
    /// Operation (<see cref="ConnectorType.Flow"/>) or input (<see cref="ConnectorType.Data"/>).
    /// </summary>
    Upstream = 1,
    /// <summary>
    /// Impulse (<see cref="ConnectorType.Flow"/>) or output (<see cref="ConnectorType.Data"/>).
    /// </summary>
    Downstream = 2,
}
