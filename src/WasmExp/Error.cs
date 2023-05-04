namespace WasmExp;

public enum Error
{
    最外部リストの外に要素があるよ,
    余計な左括弧があるよ,
    余計な右括弧があるよ,
    リストがないよ,

    バイナリが途中で終わってるよ,
    マジックナンバーがおかしいよ,
    バージョンがおかしいよ,
    セクションの順番がおかしいよ,
    セクションIdが不正だよ,
    Functionじゃないタイプコードが記されてるよ,
    ValueTypeじゃないタイプコードが記されてるよ,
    ExportKindが不正だよ,
    オペコードが不正だよ,
}

public class WasmException : Exception
{
    public Error Error { get; }

    public WasmException(Error error, Exception? innerException = null)
        : base(error.ToString(), innerException)
    {
        Error = error;
    }
}