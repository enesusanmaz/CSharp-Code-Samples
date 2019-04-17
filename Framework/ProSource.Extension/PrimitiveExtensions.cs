using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;
using System.Globalization;

namespace ProSource.Extension
{
    public static class Extensions
    {
        //#cULTUREINFO#
        private static readonly CultureInfo currentCulture = new CultureInfo("en-US");
        public static string TrimString(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return s.Trim();
        }

        public static string ToFormattedDate(this DateTime dt, string format = "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = currentCulture;
            }
            return dt.ToString(format, cultureInfo);
        }

        public static string ToFormattedDate(this DateTime? dt, string format = "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo cultureInfo = null)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToFormattedDate(format, cultureInfo);
            }
            return string.Empty;
        }

        public static string ToFormattedShortDate(this DateTime dt, string format = "dd.MM.yyyy", System.Globalization.CultureInfo cultureInfo = null)
        {
            return dt.ToFormattedDate(format);
        }

        public static string ToFormattedShortDate(this DateTime? dt, string format = "dd.MM.yyyy", System.Globalization.CultureInfo cultureInfo = null)
        {
            if (dt.HasValue)
            {
                return dt.ToFormattedDate(format);
            }

            return string.Empty;
        }

        public static DateTime? ToDateTime(this string str, string format = "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo cultureInfo = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    return null;
                var provider = System.Globalization.CultureInfo.InvariantCulture;
                if (cultureInfo != null)
                {
                    provider = cultureInfo;
                }
                var _rval = DateTime.ParseExact(str, format, provider);
                return _rval;
            }
            catch (Exception)
            {

            }
            return null;
        }

        public static string ToFormattedQuantity(this decimal val, int decimalCount = 2)
        {
            return val.ToFormattedQuantity(currentCulture, decimalCount);
        }

        public static string ToFormattedQuantity(this decimal? val, int decimalCount = 2)
        {
            return val.HasValue ? val.Value.ToFormattedQuantity(currentCulture, decimalCount) : string.Empty;
        }

        public static string ToFormattedQuantity(this decimal val, CultureInfo cultureInfo, int decimalCount = 2)
        {
            var str = "{0:#0";//{0:#,0.##}//{0:#,0.00}
            if (decimalCount > 0)
            {
                str += ".";
                for (int i = 0; i < decimalCount; i++)
                {
                    str += "#";
                }
            }
            str += "}";
            return string.Format(cultureInfo, str, val);
        }

        public static string ToFormattedQuantity(this decimal? val, CultureInfo cultureInfo, int decimalCount = 2)
        {
            return val.HasValue ? val.Value.ToFormattedQuantity(cultureInfo, decimalCount) : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val">decimal</param>
        /// <param name="cultureInfo">default culture is current ui culture</param>
        /// <returns></returns>
        public static string ToFormattedMoney(this decimal val, int decimalCount = 2, bool isDecimalCountFixed = false)
        {
            return val.ToFormattedMoney(currentCulture, decimalCount, isDecimalCountFixed);
        }

        /// <summary>
        /// Format the given decimal
        /// </summary>
        /// <param name="val">nullable decimal</param>  
        /// <param name="decimalCount">determines how many digits after currency decimal separator</param>
        /// <param name="isDecimalCountFixed">If true => After the currency decimal separator the digit is displayed as "decimalCount regardless of "val""</param>
        /// <returns></returns>
        public static string ToFormattedMoney(this decimal? val, int decimalCount = 2, bool isDecimalCountFixed = false)
        {
            return val.HasValue ? val.Value.ToFormattedMoney(decimalCount, isDecimalCountFixed) : string.Empty;
        }

        /// <summary>
        /// Format the given decimal
        /// </summary>
        /// <param name="val">decimal</param>
        /// <param name="cultureInfo">culture info</param>
        /// <param name="decimalCount">determines how many digits after currency decimal separator</param>
        /// <param name="isDecimalCountFixed">If true => After the currency decimal separator the digit is displayed as "decimalCount regardless of "val""</param>
        /// <returns></returns>
        public static string ToFormattedMoney(this decimal val, CultureInfo cultureInfo, int decimalCount = 2, bool isDecimalCountFixed = false)
        {
            var formatter = "#,0";//"0,0.00####
            if (decimalCount > 0)
            {
                formatter += ".";
                for (int i = 0; i < decimalCount; i++)
                {
                    formatter += isDecimalCountFixed ? "0" : "#";
                }
            }

            return val.ToString(formatter, cultureInfo);
        }

        /// <summary>
        /// Format the given decimal
        /// </summary>
        /// <param name="val">nullable decimal</param>  
        /// <param name="cultureInfo">culture info</param>
        /// <param name="decimalCount">determines how many digits after currency decimal separator</param>
        /// <param name="isDecimalCountFixed">If true => After the currency decimal separator the digit is displayed as "decimalCount regardless of "val""</param>
        /// <returns></returns>
        public static string ToFormattedMoney(this decimal? val, CultureInfo cultureInfo, int decimalCount = 2, bool isDecimalCountFixed = false)
        {
            return val.HasValue ? val.Value.ToFormattedMoney(cultureInfo, decimalCount, isDecimalCountFixed) : string.Empty;
        }


        public static string ToFormattedString(this decimal val, int decimalCount = 0)
        {
            var str = "{0:0";//"{0:0.###}"//"{0:0.000}"
            if (decimalCount > 0)
            {
                str += ".";
                for (int i = 0; i < decimalCount; i++)
                {
                    str += "#";
                }
            }

            str += "}";
            return string.Format(str, val);
        }

        public static string ToFormattedString(this decimal? val, int decimalCount = 0)
        {
            if (val.HasValue)
            {
                return val.Value.ToFormattedString(decimalCount: decimalCount);
            }
            return string.Empty;
        }

        public static T GetValueOrDefault<T>(this T? val, T? defaultVal = null) where T : struct
        {
            if (val.HasValue)
            {
                return val.Value;
            }
            if (defaultVal.HasValue)
            {
                return defaultVal.Value;
            }
            return default(T);
        }

        public static dynamic ToAnonymousObject(this Dictionary<string, object> dict)
        {
            var eo = new System.Dynamic.ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

            foreach (var kvp in dict)
            {
                eoColl.Add(kvp);
            }

            dynamic eoDynamic = eo;

            return eoDynamic;
        }

        public static object GetDefaultByType(this Type type)
        {
            if (type.IsValueType)
            {
                if (type.IsGenericType && type.GenericTypeArguments.Any())
                {
                    return type.GenericTypeArguments[0].GetDefaultByType();
                }
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static bool IsNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static void MapToObject<TTarget, TSource>(this TTarget _target, TSource _source)
        {
            if (_target == null)
            {
                throw new ArgumentNullException("TTarget");
            }
            if (_source == null)
            {
                return;
            }
            var _targetProps = _target.GetType().GetProperties();
            var _sourceType = _source.GetType();
            foreach (var _targetProp in _targetProps)
            {
                if (!_targetProp.CanWrite)
                {
                    continue;
                }
                var _sourceProp = _sourceType.GetProperty(_targetProp.Name);
                if (_sourceProp == null || !_sourceProp.CanRead)
                {
                    continue;
                }
                var _sourcePropType = Nullable.GetUnderlyingType(_sourceProp.PropertyType) ?? _sourceProp.PropertyType;
                var _targetPropType = Nullable.GetUnderlyingType(_targetProp.PropertyType) ?? _targetProp.PropertyType;

                if (_sourcePropType != _targetPropType)
                {
                    continue;
                }

                var _sourceVal = _sourceProp.GetValue(_source);

                if (!_targetProp.PropertyType.IsNullableType() && _sourceVal == null)
                {
                    _targetProp.SetValue(_target, _targetPropType.GetDefaultByType());
                }
                else
                {
                    _targetProp.SetValue(_target, _sourceVal);
                }

            }
        }

        //TODO: alternatif üretebiliyorsak bu metodu kullanmayalım.
        public static T ConvertTo<T>(this object obj)
        {
            T rval;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(obj.GetType()))
            {
                rval = (T)converter.ConvertFrom(obj);
            }
            else if (converter.CanConvertTo(obj.GetType()))
            {
                rval = (T)converter.ConvertTo(obj, typeof(T));
            }
            else
            {

                if (obj.GetType().Name == "JArray" || obj.GetType().Name == "Array")
                {
                    rval = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(obj.ToString());
                }
                else
                {
                    rval = (T)obj;
                }

            }
            return rval;
        }

        public static Double ToDouble(Object NumberText)
        {
            if (NumberText == null) return 0;

            String Result = ToStr(NumberText);

            try { return Convert.ToDouble(Result); }
            catch { return 0; }
        }

        public static int? ToNullable(this int? value)
        {
            if (value == 0)
                return null;
            else
                return value;
        }

        public static int? ToNullable(this int value)
        {
            if (value == 0)
                return null;
            else
                return value;
        }

        private static string ToStr(object obj)
        {
            try
            {
                if (obj == null) obj = "0";
                string Result = obj.ToString();
                for (int i = 0; i < Result.Length; i++)
                {
                    if (Result[i].ToString() != "0" &&
                        Result[i].ToString() != "1" &&
                        Result[i].ToString() != "2" &&
                        Result[i].ToString() != "3" &&
                        Result[i].ToString() != "4" &&
                        Result[i].ToString() != "5" &&
                        Result[i].ToString() != "6" &&
                        Result[i].ToString() != "7" &&
                        Result[i].ToString() != "8" &&
                        Result[i].ToString() != "9" &&
                        Result[i].ToString() != "." &&
                        Result[i].ToString() != "," &&
                        Result[i].ToString() != "-")
                    {
                        Result = Result.Remove(i, 1);
                        i--;
                    }
                }
                if (Result == "") Result = "0";
                return Result;
            }
            catch (Exception Exp) { return "0"; }
        }

        public static string ClearScriptTags(this string str)
        {
            return str.Replace("<script>", "").Replace("</script>", "");
        }

        public static string ToStringOrEmpty(this object obj)
        {
            return obj != null ? obj.ToString() : string.Empty;
        }

        public static int TryParseInt(this string txt)
        {
            int id = 0;
            int.TryParse(txt.Trim(), out id);
            return id;
        }

        /// <param name="timeZoneId">Ex: GTB Standard Time</param>
        public static DateTime ConvertByTimeZone(this DateTime currentDateTime, string timeZoneId)
        {
            string defaultTimeZoneId = AppConfig.AppConfigurationService.TimeZoneConfiguration.GetDefault();

            if (!AppConfig.AppConfigurationService.TimeZoneConfiguration.GetEnabled())
                return currentDateTime;

            if (string.IsNullOrWhiteSpace(defaultTimeZoneId) || string.IsNullOrWhiteSpace(timeZoneId))
                return currentDateTime;

            if (defaultTimeZoneId == timeZoneId)
                return currentDateTime;


            DateTime unspecifiedDateTime = new DateTime(currentDateTime.Ticks, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTime(unspecifiedDateTime,
                    TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZoneId),
                    TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
        }

        /// <param name="timeZoneId">Ex: GTB Standard Time</param>
        public static DateTime? ConvertByTimeZone(this DateTime? currentDateTime, string timeZoneId)
        {
            if (!currentDateTime.HasValue)
                return currentDateTime;

            string defaultTimeZoneId = AppConfig.AppConfigurationService.TimeZoneConfiguration.GetDefault();

            if (!AppConfig.AppConfigurationService.TimeZoneConfiguration.GetEnabled())
                return currentDateTime;

            if (string.IsNullOrWhiteSpace(defaultTimeZoneId) || string.IsNullOrWhiteSpace(timeZoneId))
                return currentDateTime;

            if (defaultTimeZoneId == timeZoneId)
                return currentDateTime;


            DateTime unspecifiedDateTime = new DateTime(currentDateTime.Value.Ticks, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTime(unspecifiedDateTime,
                    TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZoneId),
                    TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
        }

        /// <param name="timeZoneId">Ex: GTB Standard Time</param>
        public static DateTime ConvertByTimeZone(this DateTime currentDateTime, string sourceTimeZoneId, string destTimeZoneId)
        {
            DateTime unspecifiedDateTime = new DateTime(currentDateTime.Ticks, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTime(unspecifiedDateTime,
                    TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId),
                    TimeZoneInfo.FindSystemTimeZoneById(destTimeZoneId));
        }
    }
}
