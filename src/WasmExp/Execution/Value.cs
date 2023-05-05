using System.Runtime.Intrinsics;

namespace WasmExp.Execution;

internal abstract record Value;
internal abstract record NumberValue : Value;
internal abstract record VectorValue : Value;
internal abstract record ReferenceValue : Value;
internal record I32Value(int Value) : NumberValue;
internal record I64Value(long Value) : NumberValue;
internal record F32Value(float Value) : NumberValue;
internal record F64Value(double Value) : NumberValue;
internal record V128Value<T>(Vector128<T> Value) : VectorValue
    where T : struct;
internal record NullValue : ReferenceValue;
internal record InternalReferenceValue(FunctionAddress Value) : ReferenceValue;
internal record ExternalReferenceValue(ExternalAddress Value) : ReferenceValue;