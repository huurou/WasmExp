using System.Collections.Generic;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal class Store
{
    public List<FunctionInstance> Funcs { get; init; } = new();
    public List<TableInstance> Tables { get; init; } = new();
    public List<MemoryInstance> Mems { get; init; } = new();
    public List<GlobalInstance> Globals { get; init; } = new();
    public List<ElementInstance> Elems { get; init; } = new();
    public List<DataInstance> Datas { get; init; } = new();

    public FunctionInstance GetFuncinst(FunctionAddress addr)
    {
        var a = (int)addr.Value;
        return 0 <= a && a < Funcs.Count
            ? Funcs[a]
            : throw new WasmException(Error.アドレスが関数インスタンスリストの範囲外だよ);
    }
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