using WasmExp.Sections.SectionIds;

namespace WasmExp.Test;

public class Tests
{
    [Test]
    public void SctionIdTest()
    {
        // �J�X�^���Z�N�V����Id
        Assert_SectionId(0x00, SectionId.Custom);
        // �^�C�v�Z�N�V����Id
        Assert_SectionId(0x01, SectionId.Type);
        // �C���|�[�g�Z�N�V����Id
        Assert_SectionId(0x02, SectionId.Import);
        // �t�@���N�V�����Z�N�V����Id
        Assert_SectionId(0x03, SectionId.Function);
        // �e�[�u���Z�N�V����Id
        Assert_SectionId(0x04, SectionId.Table);
        // �������[�Z�N�V����Id
        Assert_SectionId(0x05, SectionId.Memory);
        // �O���[�o���Z�N�V����Id
        Assert_SectionId(0x06, SectionId.Global);
        // �G�N�X�|�[�g�Z�N�V����Id
        Assert_SectionId(0x07, SectionId.Export);
        // �X�^�[�g�Z�N�V����Id
        Assert_SectionId(0x08, SectionId.Start);
        // �G�������g�Z�N�V����Id
        Assert_SectionId(0x09, SectionId.Element);
        // �R�[�h�Z�N�V����Id
        Assert_SectionId(0x0A, SectionId.Code);
        // �f�[�^�Z�N�V����Id
        Assert_SectionId(0x0B, SectionId.Data);
        // �f�[�^�J�E���g�Z�N�V����Id
        Assert_SectionId(0x0C, SectionId.DataCount);
        // ������Id
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