namespace WasmExp.Execution;

internal record Address(int Value);
internal abstract record ExternalValue;
internal record FunctionAddress(Address Value) : ExternalValue;
internal record TableAddress(Address Value) : ExternalValue;
internal record MemoryAddress(Address Value) : ExternalValue;
internal record GlobalAddress(Address Value) : ExternalValue;
internal record ElementAddress(Address Value);
internal record DataAddress(Address Value);
internal record ExternalAddress(Address Value);