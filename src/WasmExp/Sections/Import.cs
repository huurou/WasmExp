namespace WasmExp.Sections.SectionIds;

internal record Import : SectionId
{
    public override byte Id => 0x02;
}
