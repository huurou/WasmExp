using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using WasmExp.Binary;

namespace WasmExp;

public static class Wasm
{
    public static Module Compile(string path)
    {
        return Compile(File.OpenRead(path));
    }

    public static Module Compile(Stream stream)
    {
        var module = new Module();
        var binModule = BinaryDecoder.Decode(stream);
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
    public dynamic Exports { get; } = new ExportsDynamicObject();
}

internal class ExportsDynamicObject : DynamicObject
{
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        if (binder.Name == "addTwo")
        {
            result = (int?)args?[0] + (int?)args?[1];
            return true;
        }
        result = null;
        return false;
    }
}

public class Module
{
    public List<Import> Imports { get; } = new();
    public List<Export> Exports { get; } = new();
    public List<CustomSection> CustomSections { get; } = new();

    public Module()
    {
    }

    // ファイルをコンパイルする
    public Module(string path)
    {
    }

    // ファイルをコンパイルする
    public Module(Stream stream)
    {
    }

    // 実行インタンスを作成する
    public Instance Instantiate(IEnumerable<Import>? imports = null)
    {
        imports ??= Enumerable.Empty<Import>();
        return new();
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