using System.Collections.Generic;
using System.Linq;

namespace WasmExp.Structure;

internal class Module
{
    public List<FunctionType> Types { get; init; } = new();
    public List<Function> Functions { get; init; } = new();
    public List<Table> Tables { get; init; } = new();
    public List<Memory> Memories { get; init; } = new();
    public List<Global> Globals { get; init; } = new();
    public List<Element> Elements { get; init; } = new();
    public List<Data> Datas { get; init; } = new();
    public Start? Start { get; init; }
    public List<Import> Imports { get; init; } = new();
    public List<Export> Exports { get; init; } = new();
}

internal class Function
{
    public required TypeIndex Type { get; init; }
    public List<ValueType> Locals { get; set; } = new();
    public Expression Body { get; init; } = new();
}

internal class Instruction
{
}

internal class End : Instruction
{
    public static End Singleton { get; } = new();

    private End()
    {
    }
}

internal class Table
{
    public required TableType Type { get; init; }
}

internal class Memory
{
    public required MemoryType Type { get; init; }
}

internal class Global
{
    public required GlobalType Type { get; init; }
    public required Expression Init { get; init; }
}

internal class Element
{
    public required ReferenceType Type { get; init; }
    public required List<Expression> Init { get; init; }
    public required ElementMode Mode { get; init; }
}

internal abstract record ElementMode;
internal record ElementModePassive : ElementMode;
internal record ElementModeActive(TableIndex Table, Expression Offset) : ElementMode;
internal record ElementModeDeclarative : ElementMode;

internal record Expression(IEnumerable<Instruction> Instrs)
{
    public Expression()
        : this(Enumerable.Empty<Instruction>()) { }

    public List<Instruction> Expand()
    {
        return Instrs.Append(End.Singleton).ToList();
    }
}

internal class Data
{
    public required List<byte> Init { get; init; }
    public required DataMode Mode { get; init; }
}

internal abstract record DataMode;
internal record DataModePasssive : DataMode;
internal record DataModeActive : DataMode
{
    public required MemoryIndex Memory { get; init; }
    public required Expression Offset { get; init; }
}

internal class Start
{
    public required FunctionIndex Func { get; init; }
}

internal class Export
{
    public required string Name { get; init; }
    public required ExportDesc Desc { get; init; }
}

internal abstract record ExportDesc;
internal record ExportDescFunc(FunctionIndex Value) : ExportDesc;
internal record ExportDescTable(TableIndex Value) : ExportDesc;
internal record ExportDescMemory(MemoryIndex Value) : ExportDesc;
internal record ExportDescGlobal(GlobalIndex Value) : ExportDesc;

internal class Import
{
    public required string Name { get; init; }
    public required ImportDesc Desc { get; init; }
}

internal abstract record ImportDesc;
internal record ImportDescFunc(TypeIndex Value) : ImportDesc;
internal record ImportDescTable(TableIndex Value) : ImportDesc;
internal record ImportDescMemory(MemoryIndex Value) : ImportDesc;
internal record ImportDescGlobal(GlobalIndex Value) : ImportDesc;