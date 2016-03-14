using System;
using System.IO;
using System.Linq;
using Fclp;

namespace HealthcareAnalytics.Utilities
{
    public class Program
    {
        private static readonly string DataPath;

        static Program()
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var currentDirectory = new DirectoryInfo(currentDirectoryPath);
            var projectPath = currentDirectory.Parent.Parent.Parent.Parent.FullName;

            DataPath = Path.Combine(projectPath, "data");
        }
        
        public static void Main(string[] args)
        {
            try
            {
                var inpatientClaims = CmsPatientDataUtilities.LoadInpatientData(DataPath);
                //var beneficiarySummaries = CmsPatientDataUtilities.LoadBeneficiarySummaryData(DataPath).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Data["CsvHelper"]);
            }
        }
    }
}
