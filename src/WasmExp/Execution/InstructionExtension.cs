using WasmExp.Structure;

namespace WasmExp.Execution;

internal static class InstructionExtension
{
    public static void Execute(this Instruction instruction, ExecuteContext ctx)
    {
        switch (instruction)
        {
            case Call call: call.Execute(ctx); break;
            case LocalGet localGet: localGet.Execute(ctx); break;
            case LocalSet localSet: localSet.Execute(ctx); break;
            case I32Const i32Const: i32Const.Execute(ctx); break;
            case I32Add i32Add: i32Add.Execute(ctx); break;
            case I32Sub i32Sub: i32Sub.Execute(ctx); break;

            default: break;
        }
    }

    public static void Execute(this Call call, ExecuteContext ctx)
    {
        var frame = ctx.GetCurrentFrame();
        var module = frame.Module;
        var addr = module.GetFuncAddr(call.Index);
        Auxiliary.Invoke(ctx, addr);
    }

    public static void Execute(this LocalGet localGet, ExecuteContext ctx)
    {
        var frame = ctx.GetCurrentFrame();
        ctx.Push(frame.GetLocal(localGet.Index));
    }

    public static void Execute(this LocalSet localSet, ExecuteContext ctx)
    {
        var frame = ctx.GetCurrentFrame();
        frame.SetLocal(localSet.Index, ctx.PopValue());
    }

    public static void Execute(this I32Const i32Const, ExecuteContext ctx)
    {
        ctx.Push(i32Const.Value);
    }

    public static void Execute(this I32Add i32Add, ExecuteContext ctx)
    {
        var c2 = ctx.PopValue<I32Value>();
        var c1 = ctx.PopValue<I32Value>();
        ctx.Push(new I32Value(c1.Data + c2.Data));
    }

    public static void Execute(this I32Sub i32Sub, ExecuteContext ctx)
    {
        var c2 = ctx.PopValue<I32Value>();
        var c1 = ctx.PopValue<I32Value>();
        ctx.Push(new I32Value(c1.Data - c2.Data));
    }
}