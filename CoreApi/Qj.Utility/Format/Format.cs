using System;

namespace Qj.Utility
{
    public static class Format
    {
        #region 类型转换

        /// <summary>
        /// FormatTodecimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal FormatTodecimal(this object value, decimal defaultValue = 0)
        {
            decimal a = 0;
            if(decimal.TryParse(value.ToString(),out a))
            {
                return a;
            }
            return defaultValue;
        }


        /// <summary>
        /// FormatToString
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string FormatToString(this object value, string defaultValue = "")
        {
            return value == null ? defaultValue : value.ToString();
        }


        /// <summary>
        /// 格式化Int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int FormatToInt(this object value, int defaultValue = 0)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                int parseValue;
                return int.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化Int?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? FormatToIntHasValue(this object value, int defaultValue = 0)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToInt(value, defaultValue);
            }
        }

        /// <summary>
        /// 格式化long
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long FormatToLong(this object value, long defaultValue = 0)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                long parseValue;
                return long.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化long?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long? FormatToLongHasValue(this object value, long defaultValue = 0)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToLong(value, defaultValue);
            }
        }

        /// <summary>
        /// FormatToFloat
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float FormatToFloat(this object value, float defaultValue = 0)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                float parseValue;
                return float.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化Float?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float? FormatToFloatHasValue(this object value, float defaultValue = 0)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToFloat(value, defaultValue);
            }
        }

        /// <summary>
        /// FormatToDouble
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double FormatToDouble(this object value, double defaultValue = 0)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                double parseValue;
                return double.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化Double?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double? FormatToDoubleHasValue(this object value, double defaultValue = 0)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToDouble(value, defaultValue);
            }
        }

        /// <summary>
        /// FormatToDecimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal FormatToDecimal(this object value, decimal defaultValue = 0)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                decimal parseValue;
                return decimal.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化Decimal?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal? FormatToDecimalHasValue(this object value, decimal defaultValue = 0)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToDecimal(value, defaultValue);
            }
        }

        /// <summary>
        /// FormatToBool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool FormatToBool(this object value, bool defaultValue = false)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                bool parseValue;
                return bool.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化Bool?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool? FormatToBoolHasValue(this object value, bool defaultValue = false)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToBool(value, defaultValue);
            }
        }

        /// <summary>
        /// FormatToDateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FormatToDateTime(this object value)
        {
            return FormatToDateTime(value, DateTime.MinValue);
        }

        /// <summary>
        /// FormatToDateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime FormatToDateTime(this object value, DateTime defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                DateTime parseValue;
                return DateTime.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// FormatToDateTime?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? FormatToDateTimeHasValue(this object value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToDateTime(value, DateTime.MinValue);
            }
        }

        /// <summary>
        /// FormatToGuid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid FormatToGuid(this object value)
        {
            return FormatToGuid(value, Guid.Empty);
        }

        /// <summary>
        /// FormatToGuid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid FormatToGuid(this object value, Guid defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                Guid parseValue;
                return Guid.TryParse(value.ToString(), out parseValue) ? parseValue : defaultValue;
            }
        }

        /// <summary>
        /// 格式化Guid?
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid? FormatToGuidHasValue(this object value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return FormatToGuid(value);
            }
        }

        #endregion 类型转换

        #region 字符串转枚举

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T">输入</typeparam>
        /// <param name="str"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T TryEnum<T>(this string str, T t = default(T)) where T : struct
        {
            T result;
            return Enum.TryParse(str, out result) ? result : t;
        }

        #endregion 字符串转枚举

        #region 浮点型转字符串,负数加()号

        /// <summary>
        /// 浮点型转字符串,负数加()号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DoubleToString(this double value)
        {
            return value < 0 ? string.Format("({0})", value) : value.ToString();
        }

        #endregion 浮点型转字符串,负数加()号

        /// <summary>
        /// ToUtc
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToUtc(this DateTime value)
        {
            DateTime date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt64((value - date).TotalSeconds);
            //return (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            //var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64((value - date).TotalSeconds);
        }

        /// <summary>
        /// UtcToDateTime
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns></returns>
        public static DateTime UtcToDateTime(this long utcTime)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //TimeSpan toNow = new TimeSpan(utcTime);
            //return dtStart.Add(toNow);
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtZone = dtZone.AddSeconds(utcTime);
            return dtZone.ToLocalTime();
        }
    }
}