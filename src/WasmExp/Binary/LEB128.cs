namespace WasmExp.Binary;

/// <summary>
/// Single-file utility to read and write integers in the LEB128 (7-bit little endian base-128) format.
/// See https://en.wikipedia.org/wiki/LEB128 for details.
/// </summary>
public static class LEB128
{
    private const long SIGN_EXTEND_MASK = -1L;
    private const int INT64_BITSIZE = sizeof(long) * 8;

    public static void WriteLEB128Signed(this Stream stream, long value)
    {
        stream.WriteLEB128Signed(value, out _);
    }

    public static void WriteLEB128Signed(this Stream stream, long value, out int bytes)
    {
        bytes = 0;
        var more = true;
        while (more)
        {
            var chunk = (byte)(value & 0x7fL);
            value >>= 7;
            var signBitSet = (chunk & 0x40) != 0;
            more = !(value == 0 && !signBitSet || value == -1 && signBitSet);
            if (more)
            {
                chunk |= 0x80;
            }
            stream.WriteByte(chunk);
            bytes++;
        };
    }

    public static void WriteLEB128Unsigned(this Stream stream, ulong value)
    {
        stream.WriteLEB128Unsigned(value, out _);
    }

    public static void WriteLEB128Unsigned(this Stream stream, ulong value, out int bytes)
    {
        bytes = 0;
        var more = true;
        while (more)
        {
            var chunk = (byte)(value & 0x7fUL);
            value >>= 7;
            more = value != 0;
            if (more)
            {
                chunk |= 0x80;
            }
            stream.WriteByte(chunk);
            bytes++;
        };
    }

    public static long ReadLEB128Signed(this Stream stream)
    {
        return stream.ReadLEB128Signed(out _);
    }

    public static long ReadLEB128Signed(this Stream stream, out int bytes)
    {
        bytes = 0;
        var value = 0L;
        var shift = 0;
        var more = true;
        var signBitSet = false;
        while (more)
        {
            var next = stream.ReadByte();
            if (next < 0) throw new WasmException(Error.バイナリが途中で終わってるよ);
            var b = (byte)next;
            bytes++;
            more = (b & 0x80) != 0;
            signBitSet = (b & 0x40) != 0;
            var chunk = b & 0x7fL;
            value |= chunk << shift;
            shift += 7;
        };
        if (shift < INT64_BITSIZE && signBitSet)
        {
            value |= SIGN_EXTEND_MASK << shift;
        }
        return value;
    }

    public static ulong ReadLEB128Unsigned(this Stream stream)
    {
        return stream.ReadLEB128Unsigned(out _);
    }

    public static ulong ReadLEB128Unsigned(this Stream stream, out int bytes)
    {
        bytes = 0;
        var value = 0UL;
        var shift = 0;
        var more = true;
        while (more)
        {
            var next = stream.ReadByte();
            if (next < 0) throw new WasmException(Error.バイナリが途中で終わってるよ);
            var b = (byte)next;
            bytes++;
            more = (b & 0x80) != 0;
            var chunk = b & 0x7fUL;
            value |= chunk << shift;
            shift += 7;
        }
        return value;
    }

    public static int ReadLEB128Int32(this BinaryReader br)
    {
        return (int)br.BaseStream.ReadLEB128Signed();
    }

    public static long ReadLEB128Int64(this BinaryReader br)
    {
        return br.BaseStream.ReadLEB128Signed();
    }

    public static uint ReadLEB128Uint32(this BinaryReader br)
    {
        return (uint)br.BaseStream.ReadLEB128Unsigned();
    }

    public static ulong ReadLEB128Uint64(this BinaryReader br)
    {
        return br.BaseStream.ReadLEB128Unsigned();
    }
}