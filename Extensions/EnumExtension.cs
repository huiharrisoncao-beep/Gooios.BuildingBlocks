using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Gooios.BuildingBlocks.Extensions;

public static class EnumExtension
{
    private static readonly ConcurrentDictionary<Enum, string> DescriptionCache = new();

    public static string GetDescription(this Enum value)
    {
        return DescriptionCache.GetOrAdd(value, static e =>
        {
            var field = e.GetType().GetField(e.ToString());
            if (field is null)
                return e.ToString();

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? e.ToString();
        });
    }

    public static string? GetDescriptionOrNull(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        return field?.GetCustomAttribute<DescriptionAttribute>()?.Description;
    }
}