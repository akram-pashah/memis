namespace MEMIS.Models.Risk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            return attributes.Length > 0 ? attributes[0].Name : value.ToString();
        }
    }
}
