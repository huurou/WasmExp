using WasmExp.Execution;
using WasmExp.Structure;

namespace WasmExp.Test.Execution;

public class ExecutionTest
{
    [Test]
    public void CallFuncTest()
    {
        var moduleInst = new ModuleInstance();
        var store = new Store
        {
            Funcs = new()
            {
                new()
                {
                    Type = new(new[] { ValueType.I32, ValueType.I32 }, new[] { ValueType.I32 }),
                    Module = moduleInst,
                    Code = new(new(0))
                    {
                        Body = new(new Instruction[]
                        {
                            new LocalGet(new(0)),
                            new LocalGet(new(1)),
                            new I32Sub(),
                            End.Singleton,
                        }),
                    }
                },
            },
        };
        var ctx = new ExecuteContext(store);
        var F = new Frame(0, moduleInst);
        ctx.Push(F);
        ctx.Push(new I32Value(5));
        ctx.Push(new I32Value(2));
        Auxiliary.Invoke(ctx, new(0));
        var res = ctx.PopValue();
        var poppedFrame = ctx.PopFrame();
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.EqualTo(new I32Value(3)));
            Assert.That(poppedFrame, Is.EqualTo(F));
        });
    }
}