using System;
namespace ClaimsApi.Models {
    public class PlanPayment {
        public string PrimaryPayerID { get; set; }
        public decimal CobServicePaidAmount { get; set; }
        public string ServiceCode { get; set; } // Procedure code
        public DateTime PaymentDate { get; set; }
        public string ClaimAdjGroupCode { get; set; }
        public string ClaimAdjReasonCode { get; set; }
        public decimal ClaimAdjAmount { get; set; }
        public string ClaimAdjQuantity { get; set; }
    }
}