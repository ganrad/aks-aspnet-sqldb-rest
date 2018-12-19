using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClaimsApi.Models {
    [Table("Plan_Payment")]
    public class PlanPayment {
        [Column("plan_payment_id")]
        public int PlanPaymentId { get; set; }
        [Column("primary_payer_id")]
        public string PrimaryPayerID { get; set; }
        [Column("cob_service_paid_amt", TypeName = "decimal(10,2)")]
        public decimal CobServicePaidAmount { get; set; }
        [Column("service_code")]
        public string ServiceCode { get; set; } // Procedure code
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
        [Column("claim_adj_grp_code")]
        public string ClaimAdjGroupCode { get; set; }
        [Column("claim_adj_reason_code")]
        public string ClaimAdjReasonCode { get; set; }
        [Column("claim_adj_amt", TypeName = "decimal(10,2)")]
        public decimal ClaimAdjAmount { get; set; }
        [Column("claim_adj_qty")]
        public string ClaimAdjQuantity { get; set; }
        [Required]
        [Column("claim_id")]
        public int ClaimItemId { get; set; }
    }
}