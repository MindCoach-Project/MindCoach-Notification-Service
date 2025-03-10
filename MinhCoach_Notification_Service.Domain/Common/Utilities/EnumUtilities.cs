namespace MinhCoach_Notification_Service.Domain.Common.Utilities;

public static class EnumUtilities
{
    public static string GetEnumAsString<T>()
    {
        var enumValues = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(v => $"'{v}'")  
            .ToArray();

        return string.Join(", ", enumValues);
    }
    
    public static T? ParseEnum<T>(string? value) where T : struct, Enum
    {
        if (!string.IsNullOrEmpty(value) && Enum.TryParse<T>(value, true, out var parsedEnum))
        {
            return parsedEnum;
        }
        return null;
    }
}