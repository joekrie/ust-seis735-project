using System;
using CsvHelper.TypeConversion;

namespace HealthcareAnalytics.Utilities.CsvMapping
{
    public class BooleanTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            switch (text)
            {
                case "1":
                    return true;
                case "2":
                    return false;
                case "0":
                    return false;
                case "Y":
                    return true;
                default:
                    return false;
            }
        }

        public override bool CanConvertFrom(Type type)
        {
            return type == typeof (string);
        }
    }
}