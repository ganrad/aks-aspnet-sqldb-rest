using System;

namespace ClaimsApi.Models
{
    public class ClaimItem
    {
        public long Id { get; set; }
        // 100 Record; header
        // public ClaimHeader ClaimHeader { get; set; }
        public string ClaimStatus { get; set; }
        public string ClaimType { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string OriginatorID { get; set; }
        public string DestinationID { get; set; }
        public string ClaimInputMethod { get; set; }
        // 150 Record; Subscriber info
        // public SubscriberInfo SubscriberInfo { get; set; }
        public string SubscriberRelationship { get; set; }
        public string SubscriberPolicyNumber { get; set; }
        public string InsuredGroupName { get; set; }
        public string SubscriberLastName { get; set; }
        public string SubscriberFirstName { get; set; }
        public string SubscriberMiddleName { get; set; }
        public string SubscriberIdentifierSSN { get; set; }
        public string SubscriberAddressLine1 { get; set; }
        public string SubscriberAddressLine2 { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberState { get; set; }
        public string SubscriberPostalCode { get; set; }
        public string SubscriberCountry { get; set; }
        public string SubDateOfBirth { get; set; }
        public string SubscriberGender { get; set; }
        public string PayerName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientSSN { get; set; }
        public string PatientMemberID { get; set; }
        public string PatientDOB { get; set; }
        public string PatientGender { get; set; }
        public string CatgOfService { get; set; }
        // 20I; Institutional claim record
        // public InstitutionalRecord InstitutionalRecord { get; set; }
        public string ClaimNumber { get; set; }
        public decimal TotalClaimCharge { get; set; }
        public string PatientStatus { get; set; }
        public decimal PatientAmountDue { get; set; }
        public DateTime ServiceDate { get; set; }
        // 310; Play payment info
        // public PlanPaymentInfo PlanPaymentInfo { get; set; }
        public string PolicyNumber { get; set; }
        // public string SubscriberRelationship { get; set; }

        private DateTime claimPaidDate;

        public DateTime GetClaimPaidDate()
        {
            return claimPaidDate;
        }

        public void SetClaimPaidDate(DateTime value) => claimPaidDate = value;
        // 40I; Service details
        // public ServiceLineDetails ServiceLineDetails { get; set; }
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
        // 431; Payment details
        // public PlanPayment PlanPayment { get; set; }
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