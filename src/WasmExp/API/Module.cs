using WasmExp.Binary;

namespace WasmExp;

public static class Wasm
{
    public static Module Load(string path)
    {
        return Load(File.OpenRead(path));
    }

    public static Module Load(Stream stream)
    {
        var module = new Module();
        var binModule = BinaryDecoder.Decode(stream);
        return module;
    }
}

public class Instance
{
    public dynamic? Exports { get; }
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

    // Moduleインタンスを作成する
    public Instance Instantiate(List<Import>? imports = null)
    {
        imports ??= new();
        throw new NotImplementedException();
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