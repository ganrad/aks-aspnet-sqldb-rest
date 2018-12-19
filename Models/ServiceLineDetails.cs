using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClaimsApi.Models {
    [Table("Svc_Line_Details")]
    public class ServiceLineDetails {
        [Column("service_line_id")]
        public int ServiceLineDetailsId { get; set; }
        [Column("statement_date")]
        public DateTime StatementDate { get; set; }
        [Column("line_counter")]
        public int LineCounter { get; set; }
        [Column("service_code_desc")]
        public string ServiceCodeDescription { get; set; }
        [Column("line_charge_amt", TypeName = "decimal(10,2)")]
        [System.ComponentModel.DataAnnotations.Required]
        public decimal LineChargeAmount { get; set; }
        [Column("drug_code")]
        public string DrugCode { get; set; }
        [Column("drug_unit_qty")]
        public int DrugUnitQuantity { get; set; }
        [Column("pharmacy_pres_no")]
        public string PharmacyPrescriptionNumber { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Column("service_type")]
        public string ServiceType { get; set; }
        [Column("provider_code")]
        public string ProviderCode { get; set; }
        [Column("provider_last_name")]
        public string ProviderLastName { get; set; }
        [Column("provider_first_name")]
        public string ProviderFirstName { get; set; }
        [Column("provider_id")]
        public string ProviderIdentification { get; set; }
        [Column("in_network_id")]
        public bool InNetworkIndicator { get; set; }
        [Required]
        [Column("claim_id")]
        public int ClaimItemId { get; set; }
    }
}