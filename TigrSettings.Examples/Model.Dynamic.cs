using System;

namespace Examples
{
    public interface IAppSettings
    {
        int Int { get; }
        string String { get; }
        Guid Guid { get; }
        EnumSetting Enum { get; }
        DateTime DateTime { get; }
        TimeSpan[] ArrayOfTimeSpans { get; }
        bool? NullableBoolean { get; }
        IInner Inner { get; }
        IEmpty Empty { get; }
    }

    public interface IInner
    {
        string InnerProp1 { get; }
        int?[] InnerProp2 { get; }
        IInner2 Deeper { get; }
    }

    public interface IInner2
    {
        EnumSetting InnerProp3 { get; }
    }

    public interface IEmpty
    {
    }
}
