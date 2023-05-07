namespace WasmExp.Structure;

internal record TypeIndex(uint Value) : BlockType;
internal record FunctionIndex(uint Value);
internal record TableIndex(uint Value);
internal record MemoryIndex(uint Value);
internal record GlobalIndex(uint Value);
internal record ElementIndex(uint Value);
internal record DataIndex(uint Value);
internal record LocalIndex(uint Value);
internal record LabelIndex(uint Value);