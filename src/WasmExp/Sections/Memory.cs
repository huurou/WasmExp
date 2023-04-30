namespace WasmExp.Sections.SectionIds;

internal record Memory : SectionId
{
    public override byte Id => 0x05;
}
