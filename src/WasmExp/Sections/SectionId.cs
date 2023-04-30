using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WasmExp.Sections.SectionIds;

/// <summary>
/// セクションId
/// </summary>
internal abstract record SectionId
{
    public static Custom Custom { get; }
    public static Type Type { get; }
    public static Import Import { get; }
    public static Function Function { get; }
    public static Table Table { get; }
    public static Memory Memory { get; }
    public static Global Global { get; }
    public static Export Export { get; }
    public static Start Start { get; }
    public static Element Element { get; }
    public static Code Code { get; }
    public static Data Data { get; }
    public static DataCount DataCount { get; }

    private static Dictionary<byte, SectionId> sectionIdDict_;

    static SectionId()
    {
        Custom = new();
        Type = new();
        Import = new();
        Function = new();
        Table = new();
        Memory = new();
        Global = new();
        Export = new();
        Start = new();
        Element = new();
        Code = new();
        Data = new();
        DataCount = new();
        sectionIdDict_ = typeof(SectionId)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Select(x => x.GetValue(typeof(SectionId)))
            .OfType<SectionId>()
            .ToDictionary(x => x.Id, x => x);
    }

    public abstract byte Id { get; }

    /// <summary>
    /// バイトコードからセクションIdに変換する
    /// </summary>
    /// <param name="id">セクションIdバイトコード</param>
    /// <param name="sectionId">セクションId</param>
    /// <returns>成功/失敗</returns>
    public static bool TryFromByte(byte id, [MaybeNullWhen(false)] out SectionId sectionId)
    {
        return sectionIdDict_.TryGetValue(id, out sectionId);
    }
}