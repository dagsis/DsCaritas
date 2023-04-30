using System.Reflection;

namespace Caritas.Web.Helpers
{
    public class ResourceHelper
    {
        public static string GetResourceLookup(Type resourceType, string resourceName)
        {
            if ((resourceType != null) && (resourceName != null))
            {
                PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
                if (property == null)
                {
                    return string.Empty;
                }
                if (property.PropertyType != typeof(string))
                {
                    return string.Empty;
                }

                return (string)property.GetValue(null, null)!;
            }
            return null!;
        }
    }
}
