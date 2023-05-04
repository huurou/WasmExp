namespace WasmExp.Binaries;

internal class Module
{
    private const uint WASM_MAGIC_NUMBER = 0x6d736100;
    private const uint WASM_VERSION = 0x1;

    public List<CustomSection> CustomSection { get; } = new();
    public TypeSection? TypeSection { get; private set; }
    public FunctionSection? FunctionSection { get; private set; }
    public ExportSection? ExportSection { get; private set; }
    public CodeSection? CodeSection { get; private set; }

    public Module(BinaryReader br)
    {
        CheckMagicNumber(br);
        CheckVersion(br);
        ReadSections(br);
    }

    private static void CheckMagicNumber(BinaryReader br)
    {
        if (br.ReadUInt32() != WASM_MAGIC_NUMBER)
        {
            throw new WasmException(Error.マジックナンバーがおかしいよ);
        }
    }

    private static void CheckVersion(BinaryReader br)
    {
        if (br.ReadUInt32() != WASM_VERSION)
        {
            throw new WasmException(Error.バージョンがおかしいよ);
        }
    }

    private void ReadSections(BinaryReader br)
    {
        var lastSectionId = SectionId.Custom;
        while (br.BaseStream.Position < br.BaseStream.Length)
        {
            var sectionId = (SectionId)br.ReadByte();
            if (sectionId != SectionId.Custom)
            {
                lastSectionId = sectionId <= lastSectionId
                    ? throw new WasmException(Error.セクションの順番がおかしいよ)
                    : sectionId;
            }
            var size = br.ReadLEB128Uint32();
            switch (sectionId)
            {
                case SectionId.Custom:
                    CustomSection.Add(new(br, size));
                    break;

                case SectionId.Type:
                    TypeSection = new(br);
                    break;

                case SectionId.Import:
                    break;

                case SectionId.Function:
                    FunctionSection = new(br);
                    break;

                case SectionId.Table:
                    break;

                case SectionId.Memory:
                    break;

                case SectionId.Global:
                    break;

                case SectionId.Export:
                    ExportSection = new(br);
                    break;

                case SectionId.Start:
                    break;

                case SectionId.Elem:
                    break;

                case SectionId.Code:
                    br.ReadBytes((int)size);
                    //CodeSection = new(br);
                    break;

                case SectionId.Data:
                    break;

                case SectionId.DataCount:
                    break;

                case SectionId.Tag:
                    break;

                default: throw new WasmException(Error.セクションIdが不正だよ);
            }
        }
    }
}