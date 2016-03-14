using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsvHelper;
using HealthcareAnalytics.Utilities.Entities;

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

        public static void LoadInpatientData(string dataBasePath)
        {
            const string fileNameTemplate = "DE1_0_2008_to_2010_Inpatient_Claims_Sample_{n}.csv";
            var dataPath = Path.Combine(dataBasePath, "cms/inpatient_claims");
            var filePaths = GetCsvFilePaths(dataPath, fileNameTemplate);


        }

        public static void LoadBeneficiarySummaryData(string dataBasePath)
        {
            const string fileNameBaseTemplate = "DE1_0_{year}_Beneficiary_Summary_File_Sample_{n}.csv";
            var years = new[] {2008, 2009, 2010};
            var fileNameTemplates = years.Select(y => fileNameBaseTemplate.Replace("{year}", y.ToString()));
            var dataPath = Path.Combine(dataBasePath, "cms/beneficiary_summary");
            var filePaths = fileNameTemplates.SelectMany(t => GetCsvFilePaths(dataPath, t));
            
        }

        private static IEnumerable<string> GetCsvFilePaths(string dataPath, string fileNameTemplate)
        {
            var filePathTemplate = Path.Combine(dataPath, fileNameTemplate);
            return Enumerable.Range(1, 20).Select(n => filePathTemplate.Replace("{n}", n.ToString()));
        }

        private static IEnumerable<TEntity> LoadEntitiesFromCsvFiles<TEntity>(IEnumerable<string> csvPaths)
        {
            return csvPaths.SelectMany(LoadEntitiesFromCsvFile<TEntity>);
        }

        private static IEnumerable<TEntity> LoadEntitiesFromCsvFile<TEntity>(string csvPath)
        {
            using (var reader = File.OpenText(csvPath))
            {
                var csv = new CsvReader(reader);
                csv.ConfigureCsvReader();
                return csv.GetRecords<TEntity>();
            }
        }

        private static void ConfigureCsvReader(this ICsvReader csv)
        {
            csv.Configuration.RegisterClassMap<InpatientClaimMap>();
        }
    }
}
