using WasmExp.Structure;

namespace WasmExp.Execution;

internal abstract class Instruction
{
    public abstract void Execute(ExecuteContext ctx);
}

internal class LocalGet : Instruction
{
    public LocalIndex Index { get; init; }

    public override void Execute(ExecuteContext ctx)
    {
        ctx.PushValue(ctx.GetLocal(Index));
    }
}

internal class LocalSet : Instruction
{
    public LocalIndex Index { get; init; }

    public override void Execute(ExecuteContext ctx)
    {
        ctx.SetLocal(Index, ctx.PopValue());
    }
}

internal class I32Const : Instruction
{
    public I32Value Value { get; init; }

    public I32Const(I32Value value)
    {
        Value = value;
    }

    public override void Execute(ExecuteContext ctx)
    {
        ctx.PushValue(Value);
    }
}

internal class I32Add : Instruction
{
    public override void Execute(ExecuteContext ctx)
    {
        var c2 = ctx.PopValue<I32Value>();
        var c1 = ctx.PopValue<I32Value>();
        ctx.PushValue(new I32Value(c1.Value + c2.Value));
    }
}