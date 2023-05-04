namespace WasmExp.Texts;

internal class TextDeserializer
{
    private readonly SExpressionParser sExpParser_ = new ();

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
}

internal abstract record Token;
internal record ModuleToken : Token;