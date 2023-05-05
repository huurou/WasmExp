using WasmExp.Structure;

namespace WasmExp.Execution;

internal class Store
{
}

internal class ModuleInstance
{
}

internal class FunctionInstance
{
    public FunctionType Type { get; init; }
    public ModuleInstance Module { get; init; }
    public Function Code { get; init; }
}

internal class TableInstance
{
    public TableType Type { get; init; }
    public List<ReferenceValue> Elem { get; init; } = new();
}

internal class MemoryInstance
{
    public MemoryType Type { get; init; }
    public List<byte> Data { get; init; } = new();
}

internal class GlobalInstance
{
    public GlobalType Type { get; init; }
    public Value Value { get; init; }
}

internal class ElementInstance
{
    public ReferenceType Type { get; init; }
    public List<ReferenceValue> Elem { get; init; }
}

internal class DataInstance
{
    public List<byte> Data { get; init; }
}

internal class ExportInstance
{
    public string Name { get; init; }
    public ExternalReferenceValue Value { get; init; }
}

internal class Label
{
    public int Arity { get; init; }
    public List<Instruction> Instrructions { get; init; }
}

internal class Frame
{
    public List<Value> Locals { get; init; } = new();
    public ModuleInstance Module { get; init; }
}

internal class ExecuteContext
{
    private readonly List<Value> locals_ = new();

    private readonly Stack<Value> valueStack_ = new();

    public Value GetLocal(LocalIndex index)
    {
        var i = (int)index.Value;
        return 0 <= i && i < locals_.Count
            ? locals_[i]
            : throw new IndexOutOfRangeException("ローカルインデックスが不正");
    }

    public void SetLocal(LocalIndex index, Value value)
    {
        var i = (int)index.Value;
        if (!(0 <= i && i < locals_.Count)) throw new IndexOutOfRangeException("ローカルインデックスが不正");
        if (locals_[i].GetType() != value.GetType()) throw new InvalidOperationException("型不一致");
        locals_[i] = value;
    }

    public void PushValue(Value value)
    {
        valueStack_.Push(value);
    }

    public Value PopValue()
    {
        return valueStack_.Pop();
    }

    public T PopValue<T>()
        where T : Value
    {
        return valueStack_.Pop() is T value
            ? value
            : throw new InvalidOperationException();
    }
}
