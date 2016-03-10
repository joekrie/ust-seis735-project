using System.IO;
using System.IO.Compression;
using System.Linq;

namespace HealthcareAnalytics.Utilities
{
    public static class CmsPatientDataUtilities
    {
        public static void UnzipDataFiles(string directory)
        {
            var fileNames = Directory.EnumerateFiles(directory);

            foreach (var fileName in fileNames)
            {
                ZipFile.ExtractToDirectory(fileName, directory);
            }
        }

        public static void LoadInpatientDataFromCsvFiles(string dataPath)
        {
            const string fileNameTemplate = "DE1_0_2008_to_2010_Inpatient_Claims_Sample_{n}.csv";
            var filePathTemplate = Path.Combine(dataPath, fileNameTemplate);

            var filePaths = Enumerable
                .Range(1, 20)
                .Select(n => filePathTemplate.Replace("{n}", n.ToString()));
        }
    }
}
