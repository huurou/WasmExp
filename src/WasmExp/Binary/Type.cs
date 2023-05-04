namespace WasmExp.Binary;

internal enum TypeCode
{
    I32 = 0x7F,
    I64 = 0x7E,
    F32 = 0x7D,
    F64 = 0x7C,
    V128 = 0x7B,
    I8 = 0x7A,
    I16 = 0x79,
    FuncRef = 0x70,
    ExternRef = 0x6F,
    Function = 0x60,
}

internal abstract record Type
{
    public static I32 I32 { get; } = new();
    public static I64 I64 { get; } = new();
    public static F32 F32 { get; } = new();
    public static F64 F64 { get; } = new();
    public static V128 V128 { get; } = new();
    public static I8 I8 { get; } = new();
    public static I16 I16 { get; } = new();
    public static FuncRef FuncRef { get; } = new();
    public static ExternRef ExternRef { get; } = new();

    public abstract TypeCode Code { get; }

    public static ValueType GetValueType(TypeCode code)
    {
        return code switch
        {
            TypeCode.I32 => I32,
            TypeCode.I64 => I64,
            TypeCode.F32 => F32,
            TypeCode.F64 => F64,
            TypeCode.V128 => V128,
            TypeCode.FuncRef => FuncRef,
            TypeCode.ExternRef => ExternRef,
            _ => throw new WasmException(Error.ValueTypeじゃないタイプコードが記されてるよ),
        };
    }
}

internal abstract record ValueType : Type;

internal abstract record NumberType : ValueType;

internal abstract record VectorType : ValueType;

internal abstract record ReferenceType : ValueType;

internal record I32 : NumberType
{
    public override TypeCode Code => TypeCode.I32;
}

internal record I64 : NumberType
{
    public override TypeCode Code => TypeCode.I64;
}

internal record F32 : NumberType
{
    public override TypeCode Code => TypeCode.F32;
}

internal record F64 : NumberType
{
    public override TypeCode Code => TypeCode.F64;
}

internal record V128 : VectorType
{
    public override TypeCode Code => TypeCode.V128;
}

internal record I8 : Type
{
    public override TypeCode Code => TypeCode.I8;
}

internal record I16 : Type
{
    public override TypeCode Code => TypeCode.I16;
}

internal record FuncRef : ReferenceType
{
    public override TypeCode Code => TypeCode.FuncRef;
}

internal record ExternRef : ReferenceType
{
    public override TypeCode Code => TypeCode.ExternRef;
}

internal record FunctionType : Type
{
    public override TypeCode Code => TypeCode.Function;

    public List<ValueType> Parameters { get; } = new();
    public List<ValueType> Results { get; } = new();

    public FunctionType(BinaryReader br)
    {
        var n = br.ReadLEB128Uint32();
        for (var i = 0; i < n; i++)
        {
            Parameters.Add(GetValueType((TypeCode)br.ReadByte()));
        }
        n = br.ReadLEB128Uint32();
        for (var i = 0; i < n; i++)
        {
            Results.Add(GetValueType((TypeCode)br.ReadByte()));
        }
    }

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
        foreach (var param in Parameters)
        {
            hashCode.Add(param);
        }
        foreach (var result in Results)
        {
            hashCode.Add(result);
        }
        return hashCode.ToHashCode();
    }
}