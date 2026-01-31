using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace FluxCompensator.Compiler;

public static class MethodReader
{
    private static readonly Dictionary<short, OpCode> OpCodeTable = [];
    private static readonly byte[] OperandTypeSizes;

    static MethodReader()
    {
        foreach (var field in typeof(OpCodes).GetFields())
        {
            if (field.FieldType != typeof(OpCode))
                continue;

            var code = (OpCode)field.GetValue(null)!;
            OpCodeTable.Add(code.Value, code);
        }

        OperandTypeSizes = new byte[(Enum.GetValues<OperandType>().Select(t => (int)t).Aggregate(int.Max)) + 1];
        OperandTypeSizes[(int)OperandType.InlineBrTarget] = 4;
        OperandTypeSizes[(int)OperandType.InlineField] = 4;
        OperandTypeSizes[(int)OperandType.InlineI] = 4;
        OperandTypeSizes[(int)OperandType.InlineI8] = 1;
        OperandTypeSizes[(int)OperandType.InlineMethod] = 4;
        OperandTypeSizes[(int)OperandType.InlineNone] = 0;
        OperandTypeSizes[(int)OperandType.InlineR] = 8;
        OperandTypeSizes[(int)OperandType.InlineSig] = 4;
        OperandTypeSizes[(int)OperandType.InlineString] = 4;
        OperandTypeSizes[(int)OperandType.InlineSwitch] = 4;
        OperandTypeSizes[(int)OperandType.InlineTok] = 4; // ?
        OperandTypeSizes[(int)OperandType.InlineType] = 4;
        OperandTypeSizes[(int)OperandType.InlineVar] = 2;
        OperandTypeSizes[(int)OperandType.ShortInlineBrTarget] = 1;
        OperandTypeSizes[(int)OperandType.ShortInlineI] = 1;
        OperandTypeSizes[(int)OperandType.ShortInlineR] = 4;
        OperandTypeSizes[(int)OperandType.ShortInlineVar] = 1;
    }

    public static void ReadMethod(Delegate method)
    {
        var info = method.Method;
        var body = info.GetMethodBody()
            ?? throw new InvalidOperationException("Method has no available body");
        var assembly = method.Method.Module.Assembly;
        var metadata = GetMetadataReader(assembly)
            ?? throw new InvalidOperationException("Cannot read assembly metadata");
        var il = new InstructionIterator(
            body.GetILAsByteArray() ?? throw new InvalidOperationException("Method has no body"),
            metadata
        );
        while (il.MoveNext())
        {
            Console.WriteLine($"  {il.CurrentAddress:X4}: {il.Current.ToString(metadata)}");
        }
    }

    private static unsafe MetadataReader? GetMetadataReader(Assembly asm)
    {
        if (!asm.TryGetRawMetadata(out byte* blob, out int length))
            return null;
        return new MetadataReader(blob, length);
    }

    private readonly struct Instruction(OpCode op, object? imm)
    {
        public readonly OpCode code = op;
        public readonly object? immediate = imm;

        public override string ToString() => ToString(null);

        public string ToString(MetadataReader? metadata)
        {
            if (immediate == null) return $"{code}";

            switch (code.OperandType)
            {
                case OperandType.InlineBrTarget or OperandType.ShortInlineBrTarget:
                    return $"{code} {immediate:X4}";
                default:
                    break;
            }

            if (immediate is string s)
            {
                return $"{code} \"{s}\"";
            }

            if (metadata != null)
            {
                StringHandle? name = null;

                if (immediate is MemberReference member)
                    name = member.Name;

                else if (immediate is FieldDefinition field)
                    name = field.Name;

                else if (immediate is MethodDefinition method)
                    name = method.Name;

                else if (immediate is StringHandle str)
                    name = str;

                if (name != null)
                {
                    return $"{code} \"{metadata.GetString((StringHandle)name)}\"";
                }
            }

            if (metadata != null)
            {

            }

            return $"{code} {immediate}";
        }
    }

    private ref struct InstructionIterator(byte[] data, MetadataReader metadata) : IEnumerator<Instruction>
    {
        private int _index = 0;
        private readonly ReadOnlySpan<byte> _data = data;
        private readonly MetadataReader _metadata = metadata;

        readonly object? IEnumerator.Current => Current;

        public Instruction Current { get; private set; }

        public int CurrentAddress { get; private set; }

        public bool MoveNext()
        {
            if (_index >= _data.Length) return false;
            CurrentAddress = _index;
            int code = _data[_index++];
            if (code == 0xFE)
            {
                if (_index >= _data.Length)
                    throw new InvalidDataException();
                code |= _data[_index++] << 8;
            }
            if (!OpCodeTable.TryGetValue((short)code, out var op))
                throw new InvalidDataException();
            var operandType = op.OperandType;
            var immSize = OperandTypeSizes[(int)operandType];
            if (_index + immSize > _data.Length)
                throw new InvalidDataException();
            var immData = _data.Slice(_index, immSize);
            _index += immSize;
            //Console.WriteLine(operandType);
            object? immediate = operandType switch
            {
                OperandType.InlineBrTarget => BitConverter.ToInt32(immData) + _index,
                OperandType.InlineField or
                OperandType.InlineMethod => GetTok(GetHandle(immData)),
                OperandType.InlineType => _metadata.GetTypeDefinition((TypeDefinitionHandle)GetHandle(immData)),
                OperandType.InlineTok => GetTok(GetHandle(immData)),
                OperandType.InlineSwitch or
                OperandType.InlineI => BitConverter.ToInt32(immData),
                OperandType.InlineSig => _metadata.GetStandaloneSignature((StandaloneSignatureHandle)GetHandle(immData)),
                OperandType.InlineString => _metadata.GetUserString((UserStringHandle)GetHandle(immData)),
                OperandType.InlineI8 => BitConverter.ToInt64(immData),
                OperandType.InlineNone => null,
                OperandType.InlineR => BitConverter.ToDouble(immData),
                OperandType.InlineVar => BitConverter.ToUInt16(immData),
                OperandType.ShortInlineBrTarget => (int)(sbyte)immData[0] + _index,
                OperandType.ShortInlineI => (int)(sbyte)immData[0],
                OperandType.ShortInlineR => BitConverter.ToSingle(immData),
                OperandType.ShortInlineVar => (ushort)immData[0],
                _ => throw new UnreachableException(),
            };
            Current = new(op, immediate);

            return true;
        }

        private readonly Handle GetHandle(ReadOnlySpan<byte> span) => GetHandle(BitConverter.ToInt32(span));
        private readonly Handle GetHandle(int token)
        {
            var handle = MetadataTokens.Handle(token);
            //Console.WriteLine($"Handle: {handle.Kind}");
            return handle;
        }

        private readonly object? GetTok(Handle handle) => handle.Kind switch
        {
            HandleKind.MethodDefinition => _metadata.GetMethodDefinition((MethodDefinitionHandle)handle),
            HandleKind.MethodSpecification => _metadata.GetMethodSpecification((MethodSpecificationHandle)handle),
            HandleKind.TypeDefinition => _metadata.GetTypeDefinition((TypeDefinitionHandle)handle),
            HandleKind.TypeReference => _metadata.GetTypeReference((TypeReferenceHandle)handle),
            HandleKind.TypeSpecification => _metadata.GetTypeSpecification((TypeSpecificationHandle)handle),
            HandleKind.FieldDefinition => _metadata.GetFieldDefinition((FieldDefinitionHandle)handle),
            HandleKind.MemberReference => _metadata.GetMemberReference((MemberReferenceHandle)handle),
            _ => throw new NotImplementedException($"{handle.Kind}")
        };

        public void Reset()
        {
            _index = 0;
        }

        public readonly void Dispose() { }
    }
}