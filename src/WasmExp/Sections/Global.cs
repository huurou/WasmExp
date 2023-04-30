namespace WasmExp.Sections.SectionIds;

internal record Global : SectionId
{
    public override byte Id => 0x06;
}
