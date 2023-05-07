namespace WasmExp.Execution;

internal abstract record ExternalValue(uint Value);
internal record FunctionAddress(uint Value) : ExternalValue(Value);
internal record TableAddress(uint Value) : ExternalValue(Value);
internal record MemoryAddress(uint Value) : ExternalValue(Value);
internal record GlobalAddress(uint Value) : ExternalValue(Value);
internal record ElementAddress(uint Value);
internal record DataAddress(uint Value);
internal record ExternalAddress(uint Value);