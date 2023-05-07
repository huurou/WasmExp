using System;
using System.Runtime.Intrinsics;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal abstract class Value : Entry
{
}

internal abstract class NumberValue : Value
{
}

internal abstract class VectorValue : Value
{
}

internal abstract class ReferenceValue : Value
{
}

internal class I32Value : NumberValue
{
    public int Data { get; init; }

    public I32Value(int data)
    {
        Data = data;
    }
}

internal class I64Value : NumberValue
{
    public long Data { get; init; }

    public I64Value(long data)
    {
        Data = data;
    }
}

internal class F32Value : NumberValue
{
    public float Data { get; init; }

    public F32Value(float data)
    {
        Data = data;
    }
}

internal class F64Value : NumberValue
{
    public double Data { get; init; }

    public F64Value(double data)
    {
        Data = data;
    }
}

internal class V128Value<T> : VectorValue
    where T : struct
{
    public Vector128<T> Data { get; init; }

    public V128Value(Vector128<T> data)
    {
        Data = data;
    }
}

internal class NullValue : ReferenceValue
{
    public static NullValue Singleton { get; } = new NullValue();

    private NullValue()
    {
    }
}

internal class InternalReferenceValue : ReferenceValue
{
    public FunctionAddress Data { get; init; }

    public InternalReferenceValue(FunctionAddress data)
    {
        Data = data;
    }
}

internal class ExternalReferenceValue : ReferenceValue
{
    public ExternalAddress Data { get; init; }

    public ExternalReferenceValue(ExternalAddress data)
    {
        Data = data;
    }
}

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
            FunctionReference => NullValue.Singleton,
            ExternalReference => NullValue.Singleton,
            _ => throw new NotSupportedException(),
        };
    }
}