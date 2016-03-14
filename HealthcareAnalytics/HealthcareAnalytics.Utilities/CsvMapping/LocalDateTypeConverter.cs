using System;
using System.Globalization;
using CsvHelper.TypeConversion;
using NodaTime;
using NodaTime.Text;

namespace HealthcareAnalytics.Utilities.CsvMapping
{
    public class LocalDateTypeConverter : DefaultTypeConverter
    {
        private readonly LocalDatePattern _pattern = LocalDatePattern.Create("yyyyMMdd", CultureInfo.InvariantCulture);

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            return _pattern.Parse(text);
        }
    }
}