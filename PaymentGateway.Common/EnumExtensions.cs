using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PaymentGateway.Common
{
    public static class EnumExtensions
    {

        public static List<EnumDescription> GetList<T>() where T : Enum
        {
            List<EnumDescription> Res = new List<EnumDescription>();
            foreach (T en in (T[])Enum.GetValues(typeof(T)))
            {

                var code = en.GetId();
                var description = en.GetEnumDescription();
                Res.Add(new EnumDescription() { Code = int.Parse(code.ToString()), Description = description });
            }

            return Res;

        }


        public static int GetId(this Enum value)
        {
            if (value == null)
            {
                return 0;
            }
            var val = Convert.ChangeType(value, typeof(int));
            return Convert.ToInt32(val);
        }

        public static string GetDescription(this Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }

        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var description = value.GetAttribute<DescriptionAttribute>();
            return description?.Description ?? string.Empty;
        }

        private static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute, new()
        {
            var type = value.GetType();

            var name = Enum.GetName(type, value);
            if (name == null)
                return new TAttribute();

            var field = type.GetField(name);
            if (field == null)
            {
                return new TAttribute();
            }
            return field.GetCustomAttributes(false)
                        .OfType<TAttribute>()
                        .SingleOrDefault();
        }
    }

    public class EnumDescription
    {
        public int Code { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
