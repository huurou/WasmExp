using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace WasmExp.Texts;

internal class TextSerializer
{
}

/// <summary>
/// S式を解析してSObjectのリスト構造を求める
/// </summary>
internal partial class TextParser
{
    public List<SList> Parse(string text)
    {
        text = CleanUp(text);
        var res = new List<SList>();
        var stack = new Stack<SList>();
        var buf = new StringBuilder();
        foreach (var c in text)
        {
            switch (c)
            {
                case '(':
                    AddAtom(stack, buf);
                    stack.Push(new SList());
                    break;

                case ')':
                    AddAtom(stack, buf);
                    if (!stack.TryPop(out var list)) throw new WasmException(Error.余計な右括弧があるよ);
                    if (stack.TryPeek(out var parent))
                    {
                        parent.Add(list);
                    }
                    else
                    {
                        res.Add(list);
                    }
                    break;

                case ' ':
                    AddAtom(stack, buf);
                    break;

                default:
                    buf.Append(c);
                    break;
            }
        }
        if (stack.Any()) throw new WasmException(Error.余計な左括弧があるよ);
        return res.Any() ? res : throw new WasmException(Error.リストがないよ);
    }

    private static string CleanUp(string text)
    {
        var commentRemoved = CommentRegex().Replace(text, " ");
        return BlankRegex().Replace(commentRemoved, " ").Trim();
    }

    private static void AddAtom(Stack<SList> stack, StringBuilder buf)
    {
        if (buf.Length > 0)
        {
            if (stack.TryPeek(out var list))
            {
                list.Add(new SAtom(buf.ToString()));
                buf.Clear();
            }
            else throw new WasmException(Error.最外部リストの外に要素があるよ);
        }
    }

    [GeneratedRegex(@";;.*|\(;.*;\)")]
    private static partial Regex CommentRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex BlankRegex();
}

internal record SObject;

internal record SAtom(string Value) : SObject;

internal record SList() : SObject, IEnumerable<SObject>
{
    private readonly List<SObject> list_ = new();

    public void Add(SObject obj)
    {
        list_.Add(obj);
    }

    public virtual bool Equals(SList? other)
    {
        return ReferenceEquals(this, other) ||
            other is not null &&
            EqualityContract == other.EqualityContract &&
            list_.SequenceEqual(other.list_);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(EqualityContract);
        foreach (var obj in list_)
        {
            hash.Add(obj);
        }
        return hash.ToHashCode();
    }
    public IEnumerator<SObject> GetEnumerator()
    {
        return ((IEnumerable<SObject>)list_).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)list_).GetEnumerator();
    }
}