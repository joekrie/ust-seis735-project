using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NodaTime;
using NodaTime.Text;

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

    public class InpatientClaim
    {
        public string BeneficiaryCode { get; set; }
        public string ClaimId { get; set; }
        public LocalDate ClaimsStartDate { get; set; }
        public LocalDate ClaimsEndDate { get; set; }
        public string ProviderNumber { get; set; }
        public int ClaimPaymentAmount { get; set; }
        public int NchPrimaryPayerClaimPaidAmount { get; set; }
        public string AttendingPhysicianNpi { get; set; }
        public string OperatingPhysicianNpi { get; set; }
        public string OtherPhysicianNpi { get; set; }
        public LocalDate InpatientAdmissionDate { get; set; }
        public string ClaimAdmittingDiagnosisCode { get; set; }
        public int ClaimPassThruPerDiemAmount { get; set; }
        public int NchBeneficiaryInpatientDeductibleAmount { get; set; }
        public int NchBeneficiaryPartACoinsuranceLiabilityAmount { get; set; }
        public int NchBeneficiaryBloodDeductibleLiabilityAmount { get; set; }
        public int ClaimUtilizationDayCount { get; set; }
        public LocalDate InpatientDischargedDate { get; set; }
        public string ClaimDiagnosisRelatedGroupCode { get; set; }
        public IEnumerable<string> ClaimDiagnosisCodes { get; set; }
        public IEnumerable<string> ClaimProcedureCodes { get; set; } 
        public IEnumerable<string> RevenueCenterHcfaCpcsCodes { get; set; } 
    }

    public sealed class InpatientClaimMap : CsvClassMap<InpatientClaim>
    {
        public InpatientClaimMap()
        {
            Map(c => c.BeneficiaryCode).Name("DESYNPUF_ID");
            Map(c => c.ClaimId).Name("CLM_ID");

            Map(c => c.ClaimsStartDate)
                .Name("CLM_FROM_DT")
                .TypeConverter<LocalDateTypeConverter>();

            Map(c => c.ClaimsEndDate)
                .Name("CLM_THRU_DT")
                .TypeConverter<LocalDateTypeConverter>();

            Map(c => c.ProviderNumber).Name("PRVDR_NUM");
            Map(c => c.ClaimPaymentAmount).Name("CLM_PMT_AMT");
            Map(c => c.NchPrimaryPayerClaimPaidAmount).Name("NCH_PRMRY_PYR_CLM_PD_AMT");
            Map(c => c.AttendingPhysicianNpi).Name("AT_PHYSN_NPI");
            Map(c => c.OperatingPhysicianNpi).Name("OP_PHYSN_NPI");
            Map(c => c.OtherPhysicianNpi).Name("OT_PHYSN_NPI");

            Map(c => c.InpatientAdmissionDate)
                .Name("CLM_ADMSN_DT")
                .TypeConverter<LocalDateTypeConverter>();

            Map(c => c.ClaimAdmittingDiagnosisCode).Name("ADMTNG_ICD9_DGNS_CD");
            Map(c => c.ClaimPassThruPerDiemAmount).Name("CLM_PASS_THRU_PER_DIEM_AMT");
            Map(c => c.NchBeneficiaryInpatientDeductibleAmount).Name("NCH_BENE_IP_DDCTBL_AMT");
            Map(c => c.NchBeneficiaryPartACoinsuranceLiabilityAmount).Name("NCH_BENE_PTA_COINSRNC_LBLTY_AM");
            Map(c => c.NchBeneficiaryBloodDeductibleLiabilityAmount).Name("NCH_BENE_BLOOD_DDCTBL_LBLTY_AM");
            Map(c => c.ClaimUtilizationDayCount).Name("CLM_UTLZTN_DAY_CNT");

            Map(c => c.InpatientDischargedDate)
                .Name("NCH_BENE_DSCHRG_DT")
                .TypeConverter<LocalDateTypeConverter>();

            Map(c => c.ClaimDiagnosisRelatedGroupCode).Name("CLM_DRG_CD");
            
            Map(c => c.ClaimDiagnosisCodes).ConvertUsing(r => MapStringList(r, "ICD9_DGNS_CD_", 10));
            Map(c => c.ClaimProcedureCodes).ConvertUsing(r => MapStringList(r, "ICD9_PRCDR_CD_", 6));
            Map(c => c.RevenueCenterHcfaCpcsCodes).ConvertUsing(r => MapStringList(r, "HCPCS_CD_", 45));
        }

        private static IEnumerable<string> MapStringList(ICsvReaderRow csvRow, string prefix, int maxCount)
        {
            return Enumerable
                .Range(1, maxCount)
                .Select(n => csvRow.GetField<string>($"{prefix}{n}"));
        }
    }
    
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
