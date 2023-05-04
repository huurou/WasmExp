namespace WasmExp.Binary;

internal static class BinaryDecoder
{
    public static Module Decode(Stream stream)
    {
        using var br = new BinaryReader(stream);
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