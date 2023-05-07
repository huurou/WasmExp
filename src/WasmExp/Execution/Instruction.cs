using System.Collections.Generic;
using System.Linq;
using WasmExp.Structure;

namespace WasmExp.Execution;



internal abstract class Instruction
{
    /// <summary>
    /// 命令を実行する 引数は各命令インスタンスが持っている
    /// </summary>
    /// <param name="ctx">実行コンテキスト</param>
    public abstract void Execute(ExecuteContext ctx);
}

internal static class Auxiliary
{
    public static void Invoke(ExecuteContext ctx, FunctionAddress addr)
    {
        var f = ctx.Store.GetFuncinst(addr);
        var n = f.Type.ParamCount;
        var m = f.Type.ResultCount;
        var locals = f.Code.Locals.Select(x => x.GetDefaultValue());
        var instrs = f.Code.Body;
        var vals = ctx.PopValues(n);
        var newFrame = new Frame(m, vals.Concat(locals), f.Module);
        ctx.PushFrame(newFrame);
        var label = new Label();
        Entry(ctx, label, instrs);
    }

    public static void Entry(ExecuteContext ctx, Label label, IEnumerable<Instruction> instructions)
    {
        ctx.PushLabel(label);
        foreach (var instruction in instructions)
        {
            instruction.Execute(ctx);
        }
    }
}

internal class End : Instruction
{
    public override void Execute(ExecuteContext ctx)
    {
        throw new System.NotImplementedException();
    }
}

internal class Call : Instruction
{
    public FunctionIndex Index { get; init; }

    public override void Execute(ExecuteContext ctx)
    {
        var frame = ctx.GetCurrentFrame();
        var module = frame.Module;
        var addr = module.GetFuncAddr(Index);
        Auxiliary.Invoke(ctx, addr);
    }
}

internal class LocalGet : Instruction
{
    public LocalIndex Index { get; init; }

    public override void Execute(ExecuteContext ctx)
    {
        var frame = ctx.GetCurrentFrame();
        ctx.PushValue(frame.GetLocal(Index));
    }
}

internal class LocalSet : Instruction
{
    public LocalIndex Index { get; init; }

    public override void Execute(ExecuteContext ctx)
    {
        var frame = ctx.GetCurrentFrame();
        frame.SetLocal(Index, ctx.PopValue());
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
        ctx.PushValue(new I32Value(c1.Data + c2.Data));
    }
}