[![AppVeyor Build](https://img.shields.io/appveyor/ci/sergeykonkin/SettingsMapper.svg)](https://ci.appveyor.com/project/sergeykonkin/SettingsMapper)
[![AppVeyor Tests](https://img.shields.io/appveyor/tests/sergeykonkin/SettingsMapper.svg)](https://ci.appveyor.com/project/sergeykonkin/SettingsMapper/build/tests)
[![NuGet Package](https://img.shields.io/nuget/v/SettingsMapper.svg)](https://www.nuget.org/packages/SettingsMapper)

# SettingsMapper

Simple yet extensible .NET Standard 2.0 library for mapping settings to strong types.

## Quick start

An `App.config` file:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <appSettings>
        <add key="Int" value="5" /> 
        <add key="String" value="foobar" />
        <add key="Guid" value="9d50f0fb-4127-4433-b062-7ecf211a2adb" />
        <add key="Enum" value="Foo|Baz" />
        <add key="DateTime" value="2018-04-21T12:34:56.789Z" />
        <add key="ArrayOfTimeSpans" value="00:00:05, 00:00:10, 00:00:15" />
        <add key="NullableBoolean" value="null" />
        <add key="MyType.InnerProp" value="7" />
    </appSettings>
</configuration>
```

POCO settings class:
```csharp
public class MyConfig
{
    public int Int { get; set; }
    public string String { get; set; }
    public Guid Guid { get; set; }
    public MyFlagsEnum Enum { get; set; }
    public DateTime DateTime { get; set; }
    public TimeSpan[] ArrayOfTimeSpans { get; set; }
    public bool? NullableBoolean { get; set; }
    public MyType MyType { get; set; }
}

[Flags]
public enum MyFlagsEnum
{
    Foo = 1,
    Bar = 2,
    Baz = 4
}

public class MyType
{
    public int InnerProp { get; set; }
}
```

Code:
```csharp
MyConfig config = AppSettings.MapTo<MyConfig>();
```

### Static Type Mapping

Static settings class:
```csharp
public static class MyConfig
{
    public static int Int { get; set; }
    public static string String { get; set; }
    public static Guid Guid { get; set; }
    public static MyFlagsEnum Enum { get; set; }
    public static DateTime DateTime { get; set; }
    public static TimeSpan[] ArrayOfTimeSpans { get; set; }
    public static bool? NullableBoolean { get; set; }
    public static MyType MyType { get; set; }
    public static class NestedClass
    {
        public static int NestedProp { get; set; }
    }
}
```

Code:
```csharp
AppSettings.MapToStatic(typeof(MyConfig));
```
Note that nested types are supported for static mapping

### Converters
**SettingsMapper** comes with set of default converters,  but if they are not enough, you can implement either `ISettingConverter` interface or derive from `SettingConverterBase{TValue}` class and pass additional converters to settings builder constructor:

```csharp
ISettingsProvider myConverter = new MySettingValueConverter();
ISettingsProvider myAnotherConverter = new MyAnotherSettingValueConverter();
var builder = new PocoSettingsBuilder<MyConfig>(settingsProvider, myConverter, myAnotherConverter);
```
Custom converters have higher priority than default one, so if some default converter provides insufficient functionality - it can be overwritten.

**Special case:** Built-in `NullableConverter` and `ArrayConverter` will automatically support new custom converters. For example, if custom converter provides conversion behavior for `MyType`, you don't need to create separate `MyType?` or `MyType[]` (or even `MyType?[]`) converters.


## Installation

```
Install-Package SettingsMapper
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
