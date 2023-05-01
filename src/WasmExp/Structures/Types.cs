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

internal abstract record ExternType;

internal record FuncType(IEnumerable<ValueType> Parameters, IEnumerable<ValueType> Results) : ExternType
{
    public virtual bool Equals(FuncType? other)
    {
        return ReferenceEquals(this, other) ||
            other is not null &&
            EqualityContract == other.EqualityContract &&
            Parameters.SequenceEqual(other.Parameters) &&
            Results.SequenceEqual(other.Results);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(EqualityContract);
        foreach (var parameter in Parameters)
        {
            hash.Add(parameter);
        }
        foreach (var result in Results)
        {
            hash.Add(result);
        }
        return hash.ToHashCode();
    }
}

internal record MemoryType(uint Min, uint? Max = null) : ExternType;

internal record TableType(ReferenceType ReferenceType, uint Min, uint? Max = null) : ExternType;

internal record GlobalType(ValueType ValueType, bool Mutable = false) : ExternType;