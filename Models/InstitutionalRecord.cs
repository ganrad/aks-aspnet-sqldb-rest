using System;

namespace ClaimsApi.Models {
    public class InstitutionalRecord {
        public string ClaimNumber { get; set; }
        public decimal TotalClaimCharge { get; set; }
        public string PatientStatus { get; set; }
        public decimal PatientAmountDue { get; set; }
        public DateTime ServiceDate { get; set; }
    }
}