using CsvHelper.Configuration;
using HealthcareAnalytics.Utilities.Entities;

namespace HealthcareAnalytics.Utilities.CsvMapping
{
    public sealed class BeneficiarySummaryMap : CsvClassMap<BeneficiarySummary>
    {
        public BeneficiarySummaryMap()
        {
            Map(m => m.BeneficiaryCode).Name("DESYNPUF_ID");
            Map(m => m.DateOfBirth).Name("BENE_BIRTH_DT").TypeConverter<LocalDateTypeConverter>();
            Map(m => m.DateOfDeath).Name("BENE_DEATH_DT").TypeConverter<LocalDateTypeConverter>();
            Map(m => m.Sex).Name("BENE_SEX_IDENT_CD");
            Map(m => m.BeneficiaryRace).Name("BENE_RACE_CD");
            Map(m => m.EndStageRenalDisease).Name("BENE_ESRD_IND").TypeConverter<BooleanTypeConverter>();
            Map(m => m.State).Name("SP_STATE_CODE");
            Map(m => m.CountyCode).Name("BENE_COUNTY_CD");
            Map(m => m.MonthsOfPartACoverage).Name("BENE_HI_CVRAGE_TOT_MONS");
            Map(m => m.MonthsOfPartBCoverage).Name("BENE_SMI_CVRAGE_TOT_MONS");
            Map(m => m.MonthsOfHmoCoverage).Name("BENE_HMO_CVRAGE_TOT_MONS");
            Map(m => m.MonthsOfPartDCoverage).Name("PLAN_CVRG_MOS_NUM");
            Map(m => m.AlzheimerOrSenile).Name("SP_ALZHDMTA").TypeConverter<BooleanTypeConverter>();
            Map(m => m.ChronicHeartFailure).Name("SP_CHF").TypeConverter<BooleanTypeConverter>();
            Map(m => m.ChronicKidneyFailure).Name("SP_CHRNKIDN").TypeConverter<BooleanTypeConverter>();
            Map(m => m.Cancer).Name("SP_CNCR").TypeConverter<BooleanTypeConverter>();
            Map(m => m.ChronicObstructivePulmonaryDisease).Name("SP_COPD").TypeConverter<BooleanTypeConverter>();
            Map(m => m.Depression).Name("SP_DEPRESSN").TypeConverter<BooleanTypeConverter>();
            Map(m => m.Diabetes).Name("SP_DIABETES").TypeConverter<BooleanTypeConverter>();
            Map(m => m.IschemicHeartDisease).Name("SP_ISCHMCHT").TypeConverter<BooleanTypeConverter>();
            Map(m => m.Osteoporosis).Name("SP_OSTEOPRS").TypeConverter<BooleanTypeConverter>();
            Map(m => m.RheumatoidArthritisOrOsteoarthritus).Name("SP_RA_OA").TypeConverter<BooleanTypeConverter>();
            Map(m => m.StrokeTransientIschemicAttack).Name("SP_STRKETIA").TypeConverter<BooleanTypeConverter>();
            Map(m => m.InpatientAnnualMedicareReimbursementAmount).Name("MEDREIMB_IP");
            Map(m => m.InpatientAnnualBeneficiaryResponsibilityAmount).Name("BENRES_IP");
            Map(m => m.InpatientAnnualPrimaryPayerReimbursementAmount).Name("PPPYMT_IP");
            Map(m => m.OutpatientAnnualMedicareReimbursementAmount).Name("MEDREIMB_OP");
            Map(m => m.OutpatientAnnualBeneficiaryResponsibilityAmount).Name("BENRES_OP");
            Map(m => m.OutpatientAnnualPrimaryPayerReimbursementAmount).Name("PPPYMT_OP");
            Map(m => m.CarrierAnnualMedicareReimbursementAmount).Name("MEDREIMB_CAR");
            Map(m => m.CarrierAnnualBeneficiaryResponsibilityAmount).Name("BENRES_CAR");
            Map(m => m.CarrierAnnualPrimaryPayerReimbursementAmount).Name("PPPYMT_CAR");
        }
    }
}