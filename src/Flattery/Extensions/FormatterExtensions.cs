namespace Flattery.Extensions;

internal static class FormatterExtensions
{
    public static bool IsInterfaceType(this object source, Type targetType)
    {
        foreach (var i in source.GetType().GetInterfaces())
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == targetType)
            {
                return true;
            }
        }

        return false;
    }
}
