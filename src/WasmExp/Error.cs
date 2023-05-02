namespace WasmExp;

public enum Error
{
    最外部リストの外に要素があるよ,
    余計な左括弧があるよ,
    余計な右括弧があるよ,
    リストがないよ,
}

public class WasmException : Exception
{
    public Error Error { get; }
    public override string Message => Error.ToString();

    public WasmException(Error error)
    {
        Error = error;
    }
}