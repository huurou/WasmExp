using System.Text;

namespace WasmExp.Binaries;

internal enum SectionId
{
    Custom = 0,
    Type = 1,
    Import = 2,
    Function = 3,
    Table = 4,
    Memory = 5,
    Global = 6,
    Export = 7,
    Start = 8,
    Elem = 9,
    Code = 10,
    Data = 11,
    DataCount = 12,
    Tag = 13,
}

internal class CustomSection
{
    public SectionId Id => SectionId.Custom;

    public string Name { get; }
    public byte[] Data { get; }

    public CustomSection(BinaryReader br, uint size)
    {
        var nameLength = br.ReadByte();
        Name = Encoding.UTF8.GetString(br.ReadBytes(nameLength));
        Data = br.ReadBytes((int)(size - nameLength - 1));
    }
}

internal class TypeSection
{
    public List<FunctionType> FuncTypes { get; } = new();
    public SectionId Id => SectionId.Type;

    public TypeSection(BinaryReader br)
    {
        var n = br.ReadLEB128Uint32();
        for (var i = 0; i < n; i++)
        {
            var code = (TypeCode)br.ReadByte();
            if (code == TypeCode.Function)
            {
                FuncTypes.Add(new(br));
            }
        }
    }
}

internal class FunctionSection
{
    public SectionId Id => SectionId.Function;
    public List<TypeIndex> TypeIndices { get; } = new();

    public FunctionSection(BinaryReader br)
    {
        var n = br.ReadLEB128Uint32();
        for (var i = 0; i < n; i++)
        {
            TypeIndices.Add(new(br.ReadLEB128Uint32()));
        }
    }
}

internal class ExportSection
{
    public SectionId Id => SectionId.Export;
    public List<Export> Exports { get; } = new();

    public ExportSection(BinaryReader br)
    {
        var n = br.ReadLEB128Uint32();
        for (var i = 0; i < n; i++)
        {
            Exports.Add(new(br));
        }
    }
}

internal class Export
{
    public string Name { get; private set; }
    public Index Index { get; private set; }

    public Export(BinaryReader br)
    {
        var length = br.ReadLEB128Uint32();
        Name = Encoding.UTF8.GetString(br.ReadBytes((int)length));
        var kind = (ExportKind)br.ReadByte();
        var index = br.ReadLEB128Uint32();
        Index = kind switch
        {
            ExportKind.Function => new FunctionIndex(index),
            ExportKind.Table => new TableIndex(index),
            ExportKind.Memory => new MemoryIndex(index),
            ExportKind.Global => new GlobalIndex(index),
            _ => throw new WasmException(Error.ExportKindが不正だよ),
        };
    }
}

internal enum ExportKind
{
    Function = 0x00,
    Table = 0x01,
    Memory = 0x02,
    Global = 0x03,
}

internal class CodeSection
{
    public SectionId Id => SectionId.Code;
    public List<FunctionBody> Bodys { get; } = new();

    public CodeSection(BinaryReader br)
    {
        var n = br.ReadLEB128Uint32();
        for (var i = 0; i < n; i++)
        {
            Bodys.Add(new(br));
        }
    }
}

internal class FunctionBody
{
    public List<ValueType> Locals { get; } = new();

    public FunctionBody(BinaryReader br)
    {
        var size = br.ReadLEB128Uint32();
        var declCount = br.ReadLEB128Uint32();
        for (var i = 0; i < declCount; i++)
        {
            var typeCount = br.ReadLEB128Uint32();
            var typeCode = (TypeCode)br.ReadByte();
            for (var j = 0; j < typeCount; j++)
            {
                Locals.Add(Type.GetValueType(typeCode));
            }
        }
    }
}