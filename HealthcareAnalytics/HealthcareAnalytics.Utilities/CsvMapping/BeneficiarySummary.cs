using NodaTime;

namespace HealthcareAnalytics.Utilities.CsvMapping
{
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
}