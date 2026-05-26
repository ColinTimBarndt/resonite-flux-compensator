using ProtoFiber.Core.Nodes.Operators;

namespace ProtoFiber.DotNet.Strings;

public sealed class ConcatenateStrings : Add<string, string, string>
{
    public override void Evaluate()
        => Result = Left + Right;
}

public sealed class ConcatenateStringChar : Add<string, char, string>
{
    public override void Evaluate()
        => Result = Left + Right;
}

public sealed class ConcatenateCharString : Add<char, string, string>
{
    public override void Evaluate()
        => Result = Left + Right;
}
