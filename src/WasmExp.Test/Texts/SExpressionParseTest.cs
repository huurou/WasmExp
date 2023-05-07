using System.Collections.Generic;
using WasmExp.Text;

namespace WasmExp.Test.Texts;

internal class SExpressionParseTest
{
    private SExpressionParser parser_;

    [SetUp]
    public void SetUp()
    {
        parser_ = new SExpressionParser();
    }

    [Test]
    public void SimpleParseTest()
    {
        var text = """
            (module)
            """;
        var lists = parser_.Parse(text);
        var expected = new List<SList>
        {
            new() { new SAtom("module") },
        };
        Assert.That(lists, Is.EquivalentTo(expected));
    }

    [Test]
    public void FlatParseTest()
    {
        var text = """
            (memory)
            (func)
            """;
        var lists = parser_.Parse(text);
        var expected = new List<SList>
        {
            new() { new SAtom("memory") },
            new() { new SAtom("func") },
        };
        Assert.That(lists, Is.EquivalentTo(expected));
    }

    [Test]
    public void WithCommentTest()
    {
        var text = """
            (module ;; これはモジュールです
              (func (;これは関数です;))
            )
            """;
        var lists = parser_.Parse(text);
        var expected = new List<SList>
        {
            new()
            {
                new SAtom("module"),
                new SList() { new SAtom("func") }
            },
        };
        Assert.That(lists, Is.EquivalentTo(expected));
    }

    [Test]
    public void ExistsOusideListExceptionTest()
    {
        var text = """
            aaa(module)bbb
            """;
        Assert.That(() => parser_.Parse(text), Throws.Exception.With.Message.EqualTo(Error.最外部リストの外に要素があるよ.ToString()));
    }

    [Test]
    public void UnnecessaryLeftParenExceptionTest()
    {
        var text = """
            ((module)
            """;
        Assert.That(() => parser_.Parse(text), Throws.Exception.With.Message.EqualTo(Error.余計な左括弧があるよ.ToString()));
    }

    [Test]
    public void UnnecessaryRightParenExceptionTest()
    {
        var text = """
            (module))
            """;
        Assert.That(() => parser_.Parse(text), Throws.Exception.With.Message.EqualTo(Error.余計な右括弧があるよ.ToString()));
    }

    [Test]
    public void WithoutListExceptionTest()
    {
        var text = """
            module
            """;
        Assert.That(()=>parser_.Parse(text), Throws.Exception.With.Message.EqualTo(Error.リストがないよ.ToString()));
    }
}