using System;
namespace ClaimsApi.Models {
    public class ServiceLineDetails {
        public DateTime StatementDate { get; set; }
        public int LineCounter { get; set; }
        public string ServiceCodeDescription { get; set; }
        public decimal LineChargeAmount { get; set; }
        public string DrugCode { get; set; }
        public int DrugUnitQuantity { get; set; }
        public string PharmacyPrescriptionNumber { get; set; }
        public string ServiceType { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderLastName { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderIdentification { get; set; }
        public bool InNetworkIndicator { get; set; }
    }
}