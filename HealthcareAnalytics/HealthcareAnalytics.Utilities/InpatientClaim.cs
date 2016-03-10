using System.Collections.Generic;
using NodaTime;

namespace HealthcareAnalytics.Utilities
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
}