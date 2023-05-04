namespace WasmExp.Binary;

internal static class BinaryDecoder
{
    public static Module Decode(MemoryStream ms)
    {
        using var br = new BinaryReader(ms);
        try
        {
            return new(br);
        }
        catch (EndOfStreamException)
        {
            throw new WasmException(Error.バイナリが途中で終わってるよ);
        }
    }
}