namespace WasmExp.Structures;

internal abstract record ValueType;

internal abstract record NumberType : ValueType;

internal record I32 : NumberType;

internal record I64 : NumberType;

internal record F32 : NumberType;

internal record F64 : NumberType;

internal abstract record VectorType : ValueType;

internal record V128 : VectorType;

internal abstract record ReferenceType : ValueType;

internal record Funcref : ReferenceType;

internal record Externalref : ReferenceType;

internal abstract class ExternType
{
}

internal class FunctionType : ExternType
{
    public IEnumerable<ValueType> Parameters { get; init; } = Enumerable.Empty<ValueType>();
    public IEnumerable<ValueType> Results { get; init; } = Enumerable.Empty<ValueType>();
}

internal class MemoryType : ExternType
{
    public uint Min { get; init; }
    public uint? Max { get; init; }
}

internal class TableType : ExternType
{
    public uint Min { get; init; }
    public uint? Max { get; init; }
    public required ReferenceType ReferenceType { get; init; }
}

internal class GlobalType : ExternType
{
    public bool Mutable { get; init; }
    public required ValueType ValueType { get; init; }
}