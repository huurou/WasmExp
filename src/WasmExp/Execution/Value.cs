using System;
using System.Runtime.Intrinsics;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal abstract record Value : Entry;
internal abstract record NumberValue : Value;
internal abstract record VectorValue : Value;
internal abstract record ReferenceValue : Value;
internal record I32Value(int Value) : NumberValue;
internal record I64Value(long Value) : NumberValue;
internal record F32Value(float Value) : NumberValue;
internal record F64Value(double Value) : NumberValue;
internal record V128Value<T>(Vector128<T> Value) : VectorValue
    where T : struct;
internal record NullValue : ReferenceValue
{
    public static NullValue Const { get; } = new NullValue();

    private NullValue()
    {
    }
}
internal record InternalReferenceValue(FunctionAddress Value) : ReferenceValue;
internal record ExternalReferenceValue(ExternalAddress Value) : ReferenceValue;

internal static class ValueTypeExtensions
{
    public static Value GetDefaultValue(this Structure.ValueType type)
    {
        return type switch
        {
            I32 => new I32Value(0),
            I64 => new I64Value(0),
            F32 => new F32Value(0),
            F64 => new F64Value(0),
            // V128 not yet
            FunctionReference => NullValue.Const,
            ExternalReference => NullValue.Const,
            _ => throw new NotSupportedException(),
        };
    }
}