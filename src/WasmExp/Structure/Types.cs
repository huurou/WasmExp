using System;
using System.Collections.Generic;
using System.Linq;

namespace WasmExp.Structure;

internal abstract record BlockType;
internal abstract record ValueType : BlockType
{
    public static I32 I32 => new();
    public static I64 I64 => new();
    public static F32 F32 => new();
    public static F64 F64 => new();
    public static V128 V128 => new();
    public static FunctionReference Funcref => new();
    public static ExternalReference Externref => new();

    protected ValueType()
    {
    }
}
internal abstract record NumberType : ValueType;
internal abstract record VectorType : ValueType;
internal abstract record ReferenceType : ValueType;
internal record I32 : NumberType;
internal record I64 : NumberType;
internal record F32 : NumberType;
internal record F64 : NumberType;
internal record V128 : VectorType;
internal record FunctionReference : ReferenceType;
internal record ExternalReference : ReferenceType;

internal abstract record ExternType;
internal record FunctionType(IEnumerable<ValueType> Parameters, IEnumerable<ValueType> Results) : ExternType
{
    public int ParamCount => Parameters.Count();
    public int ResultCount => Results.Count();

    public virtual bool Equals(FunctionType? other)
    {
        return ReferenceEquals(this, other) ||
            other is not null &&
            EqualityContract == other.EqualityContract &&
            Parameters.SequenceEqual(other.Parameters) &&
            Results.SequenceEqual(other.Results);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(EqualityContract);
        foreach (var parameter in Parameters)
        {
            hashCode.Add(parameter);
        }
        foreach (var result in Results)
        {
            hashCode.Add(result);
        }
        return hashCode.ToHashCode();
    }
}
internal record MemoryType(uint Min, uint? Max = null) : ExternType;
internal record TableType(ReferenceType ReferenceType, uint Min, uint? Max = null) : ExternType;
internal record GlobalType(ValueType ValueType, bool Mutable = false) : ExternType;