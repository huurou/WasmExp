using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using WasmExp.Binary;
using WasmExp.Execution;
using WasmExp.Structure;

namespace WasmExp;

public static class Wasm
{
    public static Module Compile(string path)
    {
        return Compile(File.OpenRead(path));
    }

    public static Module Compile(Stream stream)
    {
        var binModule = BinaryDecoder.Decode(stream);
        var module = new Module(binModule);
        return module;
    }

    public static Instance Instantiate(string path, IEnumerable<Import>? imports = null)
    {
        return Compile(path).Instantiate(imports);
    }

    public static Instance Instantiate(Stream stream, IEnumerable<Import>? imports = null)
    {
        return Compile(stream).Instantiate(imports);
    }
}

// Instantiateされて出来るインスタンス
public class Instance
{
    public dynamic Exports { get; }

    internal Instance(Binary.Module binModule)
    {
        Exports = new ExportsDynamicObject(binModule);
    }
}

internal class ExportsDynamicObject : DynamicObject
{
    private readonly Binary.Module binModule_;

    public ExportsDynamicObject(Binary.Module binModule)
    {
        binModule_ = binModule;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var exps = binModule_.ExportSection!.Exports;
        if (exps.FirstOrDefault(x => x.Name == binder.Name)?.Index is not Binary.FunctionIndex fi) { result = null; return false; }
        var body = binModule_.CodeSection?.FunctionBodies[(int)fi.Value];

        var moduleInst = new ModuleInstance();
        var store = new Store
        {
            Funcs = new()
            {
                new()
                {
                    Type = new(new[] { Structure.ValueType.I32, Structure.ValueType.I32 }, new[] { Structure.ValueType.I32 }),
                    Module = moduleInst,
                    Code = new(new(0))
                    {
                        Body = new(body.Instructions),
                    }
                },
            },
        };

        var ctx = new ExecuteContext(store);
        var F = new Frame(0, moduleInst);
        ctx.Push(F);
        ctx.Push(new I32Value(((int?)args[0]).Value));
        ctx.Push(new I32Value(((int?)args[1]).Value));
        Auxiliary.Invoke(ctx, new(0));
        var res = ctx.PopValue();
        result = (res as I32Value)?.Value;
        return true;
    }
}

public class Module
{
    public List<Import> Imports { get; } = new();
    public List<Export> Exports { get; } = new();
    public List<CustomSection> CustomSections { get; } = new();

    private readonly Binary.Module binModule_;

    internal Module(Binary.Module binModule)
    {
        binModule_ = binModule;
    }

    // 実行インタンスを作成する
    public Instance Instantiate(IEnumerable<Import>? imports = null)
    {
        imports ??= Enumerable.Empty<Import>();
        return new(binModule_);
    }
}

public class Import
{
    public string Module { get; init; } = "";
    public string Name { get; init; } = "";
    public ImportKind Kind { get; init; }
}

public enum ImportKind
{
    Function,
    Table,
    Memory,
    Global,
}

public class Export
{
    public string Name { get; init; } = "";
    public ExportKind ExportKind { get; init; }
}

public enum ExportKind
{
    Function,
    Table,
    Memory,
    Global,
}

public class CustomSection
{
    public string Name { get; } = "";
}

public class Global
{
}

public class Memory
{
}

public class Table
{
}