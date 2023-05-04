namespace WasmExp.Binaries;

public abstract record Index(uint Value);

public record TypeIndex(uint Value) : Index(Value);

public record FunctionIndex(uint Value) : Index(Value);

public record TableIndex(uint Value) : Index(Value);

public record MemoryIndex(uint Value) : Index(Value);

public record GlobalIndex(uint Value) : Index(Value);

public record ElementIndex(uint Value) : Index(Value);

public record DataIndex(uint Value) : Index(Value);

public record LocalIndex(uint Value) : Index(Value);

public record LabelIndex(uint Value) : Index(Value);