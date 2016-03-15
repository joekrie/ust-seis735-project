using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Fclp;
using Fclp.Internals.Extensions;

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
            DoProblem1();
            //DoProblem2();
            //DoProblem3();
            DoProblem4();

            Console.WriteLine("Press an key to exit...");
            Console.ReadKey(true);
        }

        private static void PrintBenchmarkTime(TimeSpan elapsed)
        {
            var seconds = (int) elapsed.TotalSeconds;
            Console.WriteLine($"Time to calculate: {seconds} seconds");
        }

        private static void DoProblem1()
        {
            var sw = Stopwatch.StartNew();

            var result = CmsPatientDataUtilities
                .LoadBeneficiarySummaryData(DataPath)
                .AsParallel()
                .Count(bs => bs.Depression && bs.AlzheimerOrSenile);

            sw.Stop();

            Console.WriteLine(
                "1: From all the Beneficiary-Summary files, determine how many patients have both the following chronic conditions: depression and Alzheimer's.");
            Console.WriteLine($"Result: {result}");
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem2()
        {
            var sw = Stopwatch.StartNew();

            var result = CmsPatientDataUtilities
                .LoadBeneficiarySummaryData(DataPath)
                .Count(bs => bs.Depression && bs.Diabetes);

            sw.Stop();

            Console.WriteLine(
                "2: From all the Beneficiary-Summary files, determine how many patients have both the following chronic conditions: depression and diabetes.");
            Console.WriteLine($"Result: {result}");
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem3()
        {
            var sw = Stopwatch.StartNew();

            var result = CmsPatientDataUtilities
                .LoadBeneficiarySummaryData(DataPath)
                .Count(bs => bs.Depression && bs.ChronicObstructivePulmonaryDisease);

            sw.Stop();

            Console.WriteLine(
                "3: From all the Beneficiary-Summary files, determine how many patients have both the following chronic conditions: depression and COPD.");
            Console.WriteLine($"Result: {result}");
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem4()
        {
            var sw = Stopwatch.StartNew();

            var result = CmsPatientDataUtilities
                .LoadInpatientData(DataPath)
                .AsParallel()
                .SelectMany(ip =>
                    ip.ClaimDiagnosisCodes
                        .Concat(ip.ClaimProcedureCodes)
                        .Concat(ip.RevenueCenterHcfaCpcsCodes))
                .GroupBy(code => code)
                .Select(grp => new {Icd9Code = grp.Key, Count = grp.Count()})
                .OrderByDescending(grp => grp.Count)
                .Take(10)
                .ToList();

            sw.Stop();

            Console.WriteLine(
                "4: From all the Inpatient files, determine the top 10 ICD code that have the highest frequency. Please output each code with its frequency (i.e. occurrence) in the descending order.");
            Console.WriteLine("Result:");
            result.ForEach(codeGrp => Console.WriteLine($"{codeGrp.Icd9Code}: {codeGrp.Count}"));
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }
    }
}