namespace WasmExp.Texts;

internal class TextDeserializer
{
    private readonly SExpressionParser sExpParser_ = new SExpressionParser();

    public void Deserialize(string text)
    {
        var sLists = sExpParser_.Parse(text);
    }
}

/// <summary>
/// Sリスト解析器 <para/>
/// </summary>
internal class SListParser
{
    public void Parse(SList list)
    {
    }
}

internal abstract record Token;
internal record ModuleToken : Token;