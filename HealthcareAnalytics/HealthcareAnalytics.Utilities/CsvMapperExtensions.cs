using System.Collections.Generic;
using System.Linq;
using CsvHelper;

namespace HealthcareAnalytics.Utilities
{
    public static class CsvMapperExtensions
    {
        public static IEnumerable<string> MapStringList(this ICsvReaderRow csvRow, string prefix, int maxCount)
        {
            return Enumerable
                .Range(1, maxCount)
                .Select(n => csvRow.GetField<string>($"{prefix}{n}"));
        }
    }
}