using System.Collections.Generic;
using System.Linq;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal abstract class Entry
{
}

internal class Label : Entry
{
    /// <summary>
    /// result arity
    /// </summary>
    public int Arity { get; init; }
    public List<Instruction> Instructions { get; init; } = new();
}

internal class Frame : Entry
{
    /// <summary>
    /// result arity
    /// フレーム終了時に必要な戻り値の数
    /// </summary>
    public int Arity { get; }
    public List<Value> Locals { get; }
    public ModuleInstance Module { get; }

    public Frame(int resultArity, IEnumerable<Value> locals, ModuleInstance module)
    {
        Arity = resultArity;
        Locals = locals.ToList();
        Module = module;
    }

    public Value GetLocal(LocalIndex index)
    {
        var i = (int)index.Value;
        return 0 <= i && i < Locals.Count
            ? Locals[i]
            : throw new WasmException(Error.ローカルリストの範囲外だよ);
    }

    public void SetLocal(LocalIndex index, Value value)
    {
        var i = (int)index.Value;
        Locals[i] = 0 <= i && i < Locals.Count
            ? Locals[i].GetType() == value.GetType()
                ? value
                : throw new WasmException(Error.ローカルの型が異なるよ)
            : throw new WasmException(Error.ローカルリストの範囲外だよ);
    }
}

internal class ExecuteContext
{
    public Store Store { get; init; }

    private readonly Stack<Entry> stack_ = new();

    public Entry Pop()
    {
        return stack_.TryPop(out var entry)
            ? entry
            : throw new WasmException(Error.スタックが空だよ);
    }

    public Value PopValue()
    {
        return Pop() is Value value
            ? value :
            throw new WasmException(Error.ストックトップが値じゃないよ);
    }

    public T PopValue<T>()
        where T : Value
    {
        return PopValue() is T value
            ? value
            : throw new WasmException(Error.スタックトップと要求値タイプが異なるよ);
    }

    public IEnumerable<Value> PopValues(int count)
    {
        var values = new List<Entry>();
        for (var i = 0; i < count; i++)
        {
            if (stack_.TryPop(out var entry))
            {
                values.Prepend(entry);
            }
            else throw new WasmException(Error.スタックの要素数が足りないよ);
        }
        return values.All(x => x is Value)
            ? values.OfType<Value>()
            : throw new WasmException(Error.要求された個数分の値がスタックトップから連続して存在しないよ);
    }

    public void PushValue(Value value)
    {
        stack_.Push(value);
    }

    public Label PopLabel()
    {
        return stack_.TryPop(out var entry)
            ? entry is Label label
                ? label
                : throw new WasmException(Error.スタックトップがラベルじゃないよ)
            : throw new WasmException(Error.スタックが空だよ);
    }

    public void PushLabel(Label label)
    {
        stack_.Push(label);
    }

    public Frame GetCurrentFrame()
    {
        return stack_.FirstOrDefault(x => x is Frame) is Frame frame
            ? frame
            : throw new WasmException(Error.スタックにフレームがないよ);
    }

    public Frame PopFrame()
    {
        return stack_.TryPop(out var entry)
            ? entry is Frame frame
                ? frame
                : throw new WasmException(Error.スタックトップがフレームじゃないよ)
            : throw new WasmException(Error.スタックが空だよ);
    }

    public void PushFrame(Frame frame)
    {
        stack_.Push(frame);
    }
}