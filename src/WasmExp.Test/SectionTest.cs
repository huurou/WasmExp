using WasmExp.Sections.SectionIds;

namespace WasmExp.Test;

public class Tests
{
    [Test]
    public void SctionIdTest()
    {
        // カスタムセクションId
        Assert_SectionId(0x00, SectionId.Custom);
        // タイプセクションId
        Assert_SectionId(0x01, SectionId.Type);
        // インポートセクションId
        Assert_SectionId(0x02, SectionId.Import);
        // ファンクションセクションId
        Assert_SectionId(0x03, SectionId.Function);
        // テーブルセクションId
        Assert_SectionId(0x04, SectionId.Table);
        // メモリーセクションId
        Assert_SectionId(0x05, SectionId.Memory);
        // グローバルセクションId
        Assert_SectionId(0x06, SectionId.Global);
        // エクスポートセクションId
        Assert_SectionId(0x07, SectionId.Export);
        // スタートセクションId
        Assert_SectionId(0x08, SectionId.Start);
        // エレメントセクションId
        Assert_SectionId(0x09, SectionId.Element);
        // コードセクションId
        Assert_SectionId(0x0A, SectionId.Code);
        // データセクションId
        Assert_SectionId(0x0B, SectionId.Data);
        // データカウントセクションId
        Assert_SectionId(0x0C, SectionId.DataCount);
        // 無効なId
        Assert_InvalidSectionId(0xFF);
    }

    private static void Assert_SectionId(byte id, SectionId actualSectionId)
    {
        var success = SectionId.TryFromByte(id, out var sectionId);
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(sectionId, Is.EqualTo(actualSectionId));
        });
    }

    private static void Assert_InvalidSectionId(byte id)
    {
        Assert.That(SectionId.TryFromByte(id, out _), Is.False);
    }
}