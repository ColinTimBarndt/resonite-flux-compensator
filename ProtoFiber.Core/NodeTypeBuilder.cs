namespace ProtoFiber.Core;

public sealed partial class NodeType
{

    public sealed class Builder(Type decl, bool custom)
    {

        public Builder(Type decl) : this(decl, false) { }

        internal NodeType Build(NodeTypeId id)
        {
            return new(
                id,
                _declaringType,
                _flags,
                [.. _operations],
                [.. _impulses],
                [.. _inputs, .. _outputs],
                _inputs.Count
            );
        }

        public void Reset(Type decl, bool custom)
        {
            _declaringType = decl;
            _flags = custom ? Flags.IsCustom : default;
            _operations.Clear();
            _impulses.Clear();
            _inputs.Clear();
            _outputs.Clear();
        }

        private Type _declaringType = decl;

        private Flags _flags = custom ? Flags.IsCustom : default;

        private readonly List<OperationDescriptor> _operations = [];

        private readonly List<ImpulseDescriptor> _impulses = [];

        private readonly List<DataDescriptor> _inputs = [];

        private readonly List<DataDescriptor> _outputs = [];

    }

}
