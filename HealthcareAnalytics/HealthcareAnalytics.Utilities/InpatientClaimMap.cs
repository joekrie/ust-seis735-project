using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace HealthcareAnalytics.Utilities
{
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
}