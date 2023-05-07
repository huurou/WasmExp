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
                    Type=new(new[]{ ValueType.I32, ValueType.I32 }, new[] {ValueType.I32 }),
                    Module = moduleInst,
                },
            },
        };
    }
}