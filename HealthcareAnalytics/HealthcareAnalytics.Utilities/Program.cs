using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Fclp;
using Fclp.Internals.Extensions;
using HealthcareAnalytics.Utilities.CsvMapping;
using HealthcareAnalytics.Utilities.Entities;
using Humanizer;
using NodaTime;

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
            //DoProblem1();
            //DoProblem2();
            //DoProblem3();
            //DoProblem4();
            //DoProblem5();

            NativeMethods.PreventSleep();

            Console.WriteLine("Loading inpatient claim data...");
            var sw = Stopwatch.StartNew();
            ExtractTransformLoadUtilities.LoadInpatientClaimsIntoDb(GetInpatientClaimData());
            sw.Stop();
            PrintBenchmarkTime(sw.Elapsed);

            NativeMethods.AllowSleep();
            
            Console.WriteLine("Press X to exit...");

            while (Console.ReadKey(true).Key != ConsoleKey.X) {}
        }

        private static void PrintBenchmarkTime(TimeSpan elapsed)
         => Console.WriteLine($"Time to calculate: {elapsed.Humanize(2)}");

        private static IEnumerable<BeneficiarySummary> GetBeneficiarySummaryData()
            => CmsPatientDataUtilities.LoadBeneficiarySummaryData(DataPath);

        private static IEnumerable<InpatientClaim> GetInpatientClaimData()
            => CmsPatientDataUtilities.LoadInpatientData(DataPath);

        private static void DoProblem1()
        {
            Console.WriteLine(
                "1: From all the Beneficiary-Summary files, determine how many patients have both the following chronic conditions: depression and Alzheimer's.");

            var sw = Stopwatch.StartNew();
            var result = GetBeneficiarySummaryData().Count(bs => bs.Depression && bs.AlzheimerOrSenile);
            sw.Stop();

            Console.WriteLine($"Result: {result}");
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem2()
        {
            Console.WriteLine(
                "2: From all the Beneficiary-Summary files, determine how many patients have both the following chronic conditions: depression and diabetes.");
            
            var sw = Stopwatch.StartNew();
            var result = GetBeneficiarySummaryData().Count(bs => bs.Depression && bs.Diabetes);
            sw.Stop();

            Console.WriteLine($"Result: {result}");
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem3()
        {
            Console.WriteLine(
                "3: From all the Beneficiary-Summary files, determine how many patients have both the following chronic conditions: depression and COPD.");

            var sw = Stopwatch.StartNew();
            var result = GetBeneficiarySummaryData().Count(bs => bs.Depression && bs.ChronicObstructivePulmonaryDisease);
            sw.Stop();

            Console.WriteLine($"Result: {result}");
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem4()
        {
            Console.WriteLine(
                   "4: From all the Inpatient files, determine the top 10 ICD code that have the highest frequency. Please output each code with its frequency (i.e. occurrence) in the descending order.");

            var sw = Stopwatch.StartNew();

            // Lazy-load lines from CSV file, partition them and split among PC cores to process in parallel. In each process, store
            // dictionary with ICD code as key and number of occurrences as value. After all processes finish, merge dictionaries 
            // and order descending. C#/.NET offers PLINQ (Parallel Language-Integrated Query) library to help with multithreaded
            // processing.

            var result = GetInpatientClaimData()
                .SelectMany(ip =>       // SelectMany is a flattening function
                    ip.ClaimDiagnosisCodes
                        .Concat(ip.ClaimProcedureCodes)
                        .Concat(ip.RevenueCenterHcfaCpcsCodes)
                        .Concat(new[] { ip.ClaimAdmittingDiagnosisCode }))
                .AsParallel()       // run in parallel (multi-core)
                .Aggregate(         // Aggregate defines custom aggregation function that can run in parallel
                    () => new ConcurrentDictionary<string, int>(),  // seed factory (gets called once per core used to run)
                    (codeCounts, nextCode) =>   // updateAccumulatorFunc: this function is run in parallel
                    {
                        codeCounts.AddOrUpdate(nextCode, _ => 1, (_, count) => count + 1);
                        return codeCounts;
                    },
                    (accumCounts, nextCounts) =>    // combineAccumulatorsFunc: this function combines the results of the 
                    {                               // parallel function above
                        var allCodes = accumCounts.Keys
                            .Concat(nextCounts.Keys);

                        foreach (var code in allCodes)
                        {
                            var nextCount = nextCounts.GetOrAdd(code, 0);
                            accumCounts.AddOrUpdate(code, _ => nextCount, (_, accumCount) => nextCount + accumCount);
                        }

                        return accumCounts;
                    },
                    codeCounts => codeCounts.AsParallel()   // allow further processing to run in parallel
                )
                .OrderByDescending(codeCount => codeCount.Value)
                .Take(10)   // only keep first 10
                .ToList();  

            sw.Stop();

            Console.WriteLine("Result:");
            result.ForEach(codeCount => Console.WriteLine($"{codeCount.Key}: {codeCount.Value}"));
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }

        private static void DoProblem5()
        {
            Console.WriteLine(
                "5: From all the Inpatient files and other appropriate files, determine the top 10 ICD code that have the highest re - admission frequency in 90 days.");

            var sw = Stopwatch.StartNew();

            var result = GetInpatientClaimData()
                .GroupBy(
                    ip => new {ip.BeneficiaryCode, ip.ClaimAdmittingDiagnosisCode},
                    ip => ip.InpatientAdmissionDate)
                .Select(grp =>
                {
                    var dates = grp
                        .Where(d => d.HasValue)
                        .Select(d => d.Value)
                        .ToList();

                    var readmittedInLessThan90Days = dates
                        .OrderBy(d => d)
                        .Select((date, i) => i == 0 ? 0 : Period.Between(dates[i - 1], date, PeriodUnits.Days).Days)
                        .Any(days => days > 0 && days <= 90);

                    return new
                    {
                        grp.Key.BeneficiaryCode,
                        grp.Key.ClaimAdmittingDiagnosisCode,
                        ReadmittedInLessThan90Days = readmittedInLessThan90Days
                    };
                })
                .Where(c => c.ReadmittedInLessThan90Days)
                .GroupBy(c => c.ClaimAdmittingDiagnosisCode)
                .Select(c => new
                {
                    Icd9Code = c.Key,
                    Count = c.Count()
                })
                .OrderByDescending(c => c.Count)
                .Take(10)
                .ToList();

            sw.Stop();

            Console.WriteLine("Result:");
            result.ForEach(res => Console.WriteLine($"{res.Icd9Code}: {res.Count}"));
            PrintBenchmarkTime(sw.Elapsed);
            Console.WriteLine();
        }
    }
}