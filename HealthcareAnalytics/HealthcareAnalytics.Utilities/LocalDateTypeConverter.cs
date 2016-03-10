using System;
using CsvHelper.TypeConversion;
using NodaTime;
using NodaTime.Text;

namespace HealthcareAnalytics.Utilities
{
    public class LocalDateTypeConverter : ITypeConverter
    {
        private readonly LocalDatePattern _pattern = LocalDatePattern.Create("yyyyMMdd", null);

        public string ConvertToString(TypeConverterOptions options, object value)
        {
            var localDateValue = (LocalDate) value;
            return _pattern.Format(localDateValue);
        }

        public object ConvertFromString(TypeConverterOptions options, string text)
        {
            return _pattern.Parse(text);
        }

        public bool CanConvertFrom(Type type)
        {
            return type == typeof (string) || type == typeof (int);
        }

        public bool CanConvertTo(Type type)
        {
            return type == typeof(string) || type == typeof(int);
        }
    }
}