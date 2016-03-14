using System.Collections.Generic;
using NodaTime;

namespace HealthcareAnalytics.Utilities.Entities
{
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

    public class BeneficiarySummary
    {
        public int Year { get; set; }
        public string BeneficiaryCode { get; set; }
        public LocalDate DateOfBirth { get; set; }
        public LocalDate DateOfDeath { get; set; }
        public Sex Sex { get; set; }
        public Race BeneficiaryRace { get; set; }
        public bool EndStageRenalDisease { get; set; }
        public State State { get; set; }
        public string CountyCode { get; set; }
        public int MonthsOfPartACoverage { get; set; }
        public int MonthsOfPartBCoverage { get; set; }
        public int MonthsOfHmoCoverage { get; set; }
        public int MonthsOfPartDCoverage { get; set; }
        public bool AlzheimerOrSenile { get; set; }
        public bool ChronicHeartFailure { get; set; }
        public bool ChronicKidneyFailure { get; set; }
        public bool Cancer { get; set; }
        public bool ChronicObstructivePulmonaryDisease { get; set; }
        public bool Depression { get; set; }
        public bool Diabetes { get; set; }
        public bool IschemicHeartDisease { get; set; }
        public bool Osteoporosis { get; set; }
        public bool RheumatoidArthritisOrOsteoarthritus { get; set; }
        public bool StrokeTransientIschemicAttack { get; set; }
        public int InpatientAnnualMedicareReimbursementAmount { get; set; }
        public int InpatientAnnualBeneficiaryResponsibilityAmount { get; set; }
        public int InpatientAnnualPrimaryPayerReimbursementAmount { get; set; }
        public int OutpatientAnnualMedicareReimbursementAmount { get; set; }
        public int OutpatientAnnualBeneficiaryResponsibilityAmount { get; set; }
        public int OutpatientAnnualPrimaryPayerReimbursementAmount { get; set; }
        public int CarrierAnnualMedicareReimbursementAmount { get; set; }
        public int CarrierAnnualBeneficiaryResponsibilityAmount { get; set; }
        public int CarrierAnnualPrimaryPayerReimbursementAmount { get; set; }
    }

    public enum Sex
    {
        Male = 1,
        Female = 2
    }

    public enum Race
    {
        White = 1,
        Black = 2,
        Others = 3,
        Hispanic = 5
    }

    public enum State
    {
        AL = 1,
        AK = 2,
        AZ = 3,
        AR = 4,
        CA = 5,
        CO = 6,
        CT = 7,
        DE = 8,
        DC = 9,
        FL = 10,
        GA = 11,
        HI = 12,
        ID = 13,
        IL = 14,
        IN = 15,
        IA = 16,
        KS = 17,
        KY = 18,
        LA = 19,
        ME = 20,
        MD = 21,
        MA = 22,
        MI = 23,
        MN = 24,
        MS = 25,
        MO = 26,
        MT = 27,
        NE = 28,
        NV = 29,
        NH = 30,
        NJ = 31,
        NM = 32,
        NY = 33,
        NC = 34,
        ND = 35,
        OH = 36,
        OK = 37,
        OR = 38,
        PA = 39,
        RI = 41,
        SC = 42,
        SD = 43,
        TN = 44,
        TX = 45,
        UT = 46,
        VT = 47,
        VA = 49,
        WA = 50,
        WV = 51,
        WI = 52,
        WY = 53,
        Others = 54
    }
}