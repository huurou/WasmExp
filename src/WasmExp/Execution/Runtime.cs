using System.Collections.Generic;
using System.Linq;
using WasmExp.Structure;

namespace WasmExp.Execution;

internal abstract record Entry;

internal record Label(int Arity) : Entry
{
    public List<Instruction> Instructions { get; init; } = new();

    public Label(int arity, IEnumerable<Instruction>? instructions = null)
        : this(arity)
    {
        Instructions = instructions?.ToList() ?? new();
    }
}

internal record Frame(int Arity, ModuleInstance Module) : Entry
{
    public List<Value> Locals { get; init; } = new();

    public Frame(int arity, ModuleInstance module, IEnumerable<Value>? locals = null)
        : this(arity, module)
    {
        Arity = arity;
        Module = module;
        Locals = locals?.ToList() ?? new();
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

    public ExecuteContext(Store store)
    {
        Store = store;
    }

    public Entry Pop()
    {
        return stack_.TryPop(out var entry)
            ? entry
            : throw new WasmException(Error.スタックが空だよ);
    }

    public void Push(Entry entry)
    {
        stack_.Push(entry);
    }

    public void Push(IEnumerable<Entry> entries)
    {
        foreach (var entry in entries)
        {
            Push(entry);
        }
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

    public IEnumerable<Value> PopValues()
    {
        var values = new List<Value>();
        while (true)
        {
            if (stack_.TryPop(out var entry))
            {
                if (entry is Value value)
                {
                    values.Add(value);
                }
                else
                {
                    stack_.Push(entry);
                    return values;
                }
            }
            else throw new WasmException(Error.スタックが空か値以外の要素がなかったよ);
        }
    }

    public IEnumerable<Value> PopValues(int count)
    {
        var entries = new List<Entry>();
        for (var i = 0; i < count; i++)
        {
            if (stack_.TryPop(out var entry))
            {
                entries.Add(entry);
            }
            else throw new WasmException(Error.スタックの要素数が足りないよ);
        }
        return entries.All(x => x is Value)
            ? entries.OfType<Value>().Reverse()
            : throw new WasmException(Error.要求された個数分の値がスタックトップから連続して存在しないよ);
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
}