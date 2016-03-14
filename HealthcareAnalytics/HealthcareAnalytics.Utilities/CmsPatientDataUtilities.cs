using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsvHelper;
using HealthcareAnalytics.Utilities.CsvMapping;

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

        public static IEnumerable<InpatientClaim> LoadInpatientData(string dataBasePath)
        {
            const string fileNameTemplate = "DE1_0_2008_to_2010_Inpatient_Claims_Sample_{n}.csv";
            var dataPath = Path.Combine(dataBasePath, "cms/inpatient_claims");
            var filePaths = GetCsvFilePaths(dataPath, fileNameTemplate);
            return LoadEntitiesFromCsvFiles<InpatientClaim>(filePaths);
        }

        public static IEnumerable<BeneficiarySummary> LoadBeneficiarySummaryData(string dataBasePath)
        {
            const string fileNameBaseTemplate = "DE1_0_{year}_Beneficiary_Summary_File_Sample_{n}.csv";
            var dataPath = Path.Combine(dataBasePath, "cms/beneficiary_summary");

            return new[] {2008, 2009, 2010}
                .SelectMany(y =>
                {
                    var fileNameTemplate = fileNameBaseTemplate.Replace("{year}", y.ToString());
                    var csvPaths = GetCsvFilePaths(dataPath, fileNameTemplate);

                    return LoadEntitiesFromCsvFiles<BeneficiarySummary>(csvPaths)
                        .Select(e =>
                        {
                            e.Year = y;
                            return e;
                        });
                });
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
                return csv.GetRecords<TEntity>().ToList();
            }
        }

        private static void ConfigureCsvReader(this ICsvReader csv)
        {
            csv.Configuration.RegisterClassMap<InpatientClaimMap>();
            csv.Configuration.RegisterClassMap<BeneficiarySummaryMap>();
        }
    }
}
