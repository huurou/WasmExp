using System.Linq;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal static class Auxiliary
{
    public static void Invoke(ExecuteContext ctx, FunctionAddress addr)
    {
        var f = ctx.Store.GetFuncinst(addr);
        var n = f.Type.ParamCount;
        var m = f.Type.ResultCount;
        var locals = f.Code.Locals.Select(x => x.GetDefaultValue());
        var expr = f.Code.Body;
        var vals = ctx.PopValues(n);
        var newFrame = new Frame(m, f.Module, vals.Concat(locals));
        ctx.Push(newFrame);
        var label = new Label(0);
        Entry(ctx, label, expr);
    }

    public static void Entry(ExecuteContext ctx, Label label, Expression expr)
    {
        ctx.PushLabel(label);
        foreach (var instr in expr.Instrs)
        {
            instr.Execute(ctx);
        }
        Exit(ctx, label);
    }

    public static void Execute(ExecuteContext ctx, Instruction instr)
    {
    }

    public static void Exit(ExecuteContext ctx, Label label)
    {
        var vals = ctx.PopValues();
        if (ctx.PopLabel() != label) throw new WasmException(Error.スタックトップのラベルが不正だよ);
        ctx.Push(vals);
        Return(ctx);
    }

    public static void Return(ExecuteContext ctx)
    {
        var F = ctx.GetCurrentFrame();
        var n = F.Arity;
        var vals = ctx.PopValues(n);
        if (ctx.PopFrame() != F) throw new WasmException(Error.スタックトップのフレームが不正だよ);
        ctx.Push(vals);
    }
}