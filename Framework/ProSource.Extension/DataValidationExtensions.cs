using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace ProSource.Extension
{
    public static class DataValidationExtensions
    {
        //#CULTUREINFO#
        private static readonly CultureInfo currentCulture = new CultureInfo("en-US");
        private static readonly string dateFormat = "dd.MM.yyyy";

        public static bool IsValidDate(this string value, CultureInfo cultureInfo = null)
        {
            if (value == null) throw new ArgumentNullException("value");

            DateTime date = DateTime.MinValue;
            return DateTime.TryParseExact(value, dateFormat, cultureInfo ?? CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date);
        }

        public static bool IsValidDate(this string value, out DateTime resultDate, CultureInfo cultureInfo = null)
        {
            if (value == null) throw new ArgumentNullException("value");

            return DateTime.TryParseExact(value, dateFormat, cultureInfo ?? CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out resultDate);
        }

        public static DateTime ToDate(this string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            DateTime date = DateTime.MinValue;
            if (DateTime.TryParseExact(value.Substring(0, 10), dateFormat, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                return date;
            else
                throw new ArgumentException("value property date format is invalid."); //Eger tarihi ceviremezse minDate donmemesi icin koydum.
        }
        public static bool IsValidDecimal(this string value, CultureInfo cultureInfo = null)
        {
            if (value == null) throw new ArgumentNullException("value");

            decimal temp;

            return decimal.TryParse(value, NumberStyles.Float, cultureInfo?.NumberFormat ?? currentCulture.NumberFormat, out temp);
        }
        public static decimal ToDecimal(this string value, NumberStyles numberStyles = NumberStyles.Float)
        {
            if (value == null) throw new ArgumentNullException("value");

            decimal temp;

            if (decimal.TryParse(value, numberStyles, currentCulture.NumberFormat, out temp))
                return temp;
            else
                throw new Exception(value + " ondalık format için uygun değil");
        }

        public static bool TryParseDecimal(this string value, out decimal decimalValue, NumberStyles numberStyles = NumberStyles.Float)
        {
            return decimal.TryParse(value, numberStyles, currentCulture.NumberFormat, out decimalValue);
        }

        public static bool IsValidEmail(this string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsValidURL(this string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return Regex.IsMatch(value, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");
        }

        public static bool IsValidDUNSNumber(this string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return Regex.IsMatch(value, @"^\d{9}$");
        }
    }
}
