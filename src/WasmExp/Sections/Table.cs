namespace WasmExp.Sections.SectionIds;

internal record Table : SectionId
{
    public override byte Id => 0x04;
}
