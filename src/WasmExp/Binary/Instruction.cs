namespace WasmExp.Binary;

internal enum Prefix
{
    GC_String = 0xFB,
    FC = 0xFC,
    SIMD = 0xFD,
    Thread = 0xFE,
}

internal enum NormalOpCode
{
    End = 0x0B,
    LocalGet = 0x20,
    LocalSet = 0x21,
    I32Const = 0x41,
    I32Add = 0x6A,
}

internal record Instruction
{
    public static Instruction GetInstruction(BinaryReader br, ref bool more)
    {
        var code = br.ReadByte();
        return (Prefix)code switch
        {
            Prefix.GC_String => throw new NotSupportedException(),
            Prefix.FC => throw new NotSupportedException(),
            Prefix.SIMD => throw new NotSupportedException(),
            Prefix.Thread => throw new NotSupportedException(),
            _ => GetNormalInstruction(code, br, ref more),
        };
    }

    private static Instruction GetNormalInstruction(byte code, BinaryReader br, ref bool more)
    {
        more = true;
        switch ((NormalOpCode)code)
        {
            case NormalOpCode.End:
                more = false;
                return new End();

            case NormalOpCode.LocalGet:
                return new LocalGet(br);

            case NormalOpCode.LocalSet:
                return new LocalSet(br);

            case NormalOpCode.I32Const:
                return new I32Const(br);

            case NormalOpCode.I32Add:
                return new I32Add();

            default:
                throw new WasmException(Error.オペコードが不正だよ);
        }
    }
}