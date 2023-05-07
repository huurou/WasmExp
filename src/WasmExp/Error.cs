using System;

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

    ローカルリストの範囲外だよ,
    ローカルの型が異なるよ,
    スタックが空だよ,
    ストックトップが値じゃないよ,
    スタックトップと要求値タイプが異なるよ,
    スタックが空か値以外の要素がなかったよ,
    スタックの要素数が足りないよ,
    要求された個数分の値がスタックトップから連続して存在しないよ,
    スタックトップがフレームじゃないよ,
    スタックにフレームがないよ,
    スタックトップがラベルじゃないよ,
    スタックトップのラベルが不正だよ,
    スタックトップのフレームが不正だよ,
    関数アドレスリストの範囲外だよ,
    アドレスが関数インスタンスリストの範囲外だよ,
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