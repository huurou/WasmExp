using System.IO;

namespace WasmExp.Binary;

internal abstract record NormalInstruction : Instruction
{
    public abstract NormalOpCode OpCode { get; }
}

internal record End : NormalInstruction
{
    public override NormalOpCode OpCode => NormalOpCode.End;
}

internal record LocalGet : NormalInstruction
{
    public override NormalOpCode OpCode => NormalOpCode.LocalGet;
    public LocalIndex LocalIndex { get; init; }

    public LocalGet(LocalIndex localIndex)
    {
        LocalIndex = localIndex;
    }

    public LocalGet(BinaryReader br)
    {
        LocalIndex = new(br.ReadLEB128Uint32());
    }
}

internal record LocalSet : NormalInstruction
{
    public override NormalOpCode OpCode => NormalOpCode.LocalSet;
    public LocalIndex LocalIndex { get; init; }

    public LocalSet(LocalIndex localIndex)
    {
        LocalIndex = localIndex;
    }

    public LocalSet(BinaryReader br)
    {
        LocalIndex = new(br.ReadLEB128Uint32());
    }
}

internal record I32Const : NormalInstruction
{
    public override NormalOpCode OpCode => NormalOpCode.I32Const;
    public int Value { get; init; }

    public I32Const(int value)
    {
        Value = value;
    }

    public I32Const(BinaryReader br)
    {
        Value = br.ReadLEB128Int32();
    }
}

internal record I32Add : NormalInstruction
{
    public override NormalOpCode OpCode => NormalOpCode.I32Add;
}