namespace WasmExp.Sections.SectionIds;

internal record Export : SectionId
{
    public override byte Id => 0x07;
}
