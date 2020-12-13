using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Qj.Utility.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }

        /// <summary>
        /// 扩展方法，获得枚举的Value
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举的Description</returns>
        public static int GetValue(this Enum value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 扩展方法，获得枚举的Name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetName(this Enum value)
        {
            try
            {
                return value.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static T ToEnum<T>(int value, T defaultT) where T : struct
        {
            string enumName = Enum.GetName(typeof(T), value);

            return ToEnum<T>(enumName, defaultT);
        }

        public static T ToEnum<T>(string enumName, T defaultT) where T : struct
        {
            if (string.IsNullOrWhiteSpace(enumName))
            {
                return defaultT;
            }

            T result;

            if (!Enum.TryParse<T>(enumName.Trim(), out result))
            {
                return defaultT;
            }

            if (Enum.IsDefined(typeof(T), result))
            {
                return result;
            }

            return defaultT;
        }

        public static List<EnumEntity> EnumToList<T>()
        {
            List<EnumEntity> list = new List<EnumEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.Description = da.Description;
                }
                m.Value = Convert.ToInt32(e);
                m.Name = e.ToString();
                list.Add(m);
            }
            return list;
        }
    }

    /// <summary>
    /// 枚举值类型
    /// </summary>
    [DisplayName("枚举值类型")]
    public class EnumEntity
    {
        /// <summary>
        /// 枚举的描述
        /// </summary>
        [DisplayName("枚举的描述")]
        public string Description { set; get; }

        /// <summary>
        /// 枚举名称
        /// </summary>
        [DisplayName("枚举名称")]
        public string Name { set; get; }

        /// <summary>
        /// 枚举对象的值
        /// </summary>
        [DisplayName("枚举对象的值")]
        public int Value { set; get; }
    }
}