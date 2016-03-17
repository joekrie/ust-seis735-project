using System.Collections.Generic;
using NodaTime;

namespace HealthcareAnalytics.Utilities.Entities
{
    public class InpatientClaim
    {
        public string BeneficiaryCode { get; set; }
        public string ClaimId { get; set; }
        public LocalDate? ClaimsStartDate { get; set; }
        public LocalDate? ClaimsEndDate { get; set; }
        public string ProviderNumber { get; set; }
        public decimal? ClaimPaymentAmount { get; set; }
        public decimal? NchPrimaryPayerClaimPaidAmount { get; set; }
        public string AttendingPhysicianNpi { get; set; }
        public string OperatingPhysicianNpi { get; set; }
        public string OtherPhysicianNpi { get; set; }
        public LocalDate? InpatientAdmissionDate { get; set; }
        public string ClaimAdmittingDiagnosisCode { get; set; }
        public decimal? ClaimPassThruPerDiemAmount { get; set; }
        public decimal? NchBeneficiaryInpatientDeductibleAmount { get; set; }
        public decimal? NchBeneficiaryPartACoinsuranceLiabilityAmount { get; set; }
        public decimal? NchBeneficiaryBloodDeductibleLiabilityAmount { get; set; }
        public int? ClaimUtilizationDayCount { get; set; }
        public LocalDate? InpatientDischargedDate { get; set; }
        public string ClaimDiagnosisRelatedGroupCode { get; set; }
        public IEnumerable<string> ClaimDiagnosisCodes { get; set; }
        public IEnumerable<string> ClaimProcedureCodes { get; set; }
        public IEnumerable<string> RevenueCenterHcfaCpcsCodes { get; set; }
    }
}