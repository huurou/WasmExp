using System.Collections.Generic;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal class ModuleInstance
{
    public List<FunctionType> Types { get; init; } = new();
    public List<FunctionAddress> Funcaddrs { get; init; } = new();
    public List<MemoryAddress> Memaddrs { get; init; } = new();
    public List<GlobalAddress> Globaladdrs { get; init; } = new();
    public List<ElementAddress> Elemaddrs { get; init; } = new();
    public List<DataAddress> Dataaddrs { get; init; } = new();
    public List<ExportInstance> Exportinst { get; init; } = new();

    public FunctionAddress GetFuncAddr(FunctionIndex index)
    {
        var i = (int)index.Value;
        return 0 <= i && i < Funcaddrs.Count
            ? Funcaddrs[i]
            : throw new WasmException(Error.関数アドレスリストの範囲外だよ);
    }
}

internal class ExportInstance
{
    public string Name { get; init; }
    public ExternalReferenceValue Value { get; init; }
}