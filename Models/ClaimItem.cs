using System;
using System.Collections.Generic;

namespace ClaimsApi.Models
{
    public class ClaimItem
    {
        public int ClaimItemId { get; set; }
        // 100 Record; header
        public string ClaimStatus { get; set; }
        public string ClaimType { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string OriginatorID { get; set; }
        public string DestinationID { get; set; }
        public string ClaimInputMethod { get; set; }
        // 150 Record; Subscriber info
        public List<SubscriberInfo> SubscriberInfo { get; set; }
        // 20I; Institutional claim record
        public string ClaimNumber { get; set; }
        public decimal TotalClaimCharge { get; set; }
        public string PatientStatus { get; set; }
        public decimal PatientAmountDue { get; set; }
        public DateTime ServiceDate { get; set; }
        // 310; Play payment info
        public string PolicyNumber { get; set; }
        // public string SubscriberRelationship { get; set; }

        private DateTime claimPaidDate;
        public DateTime ClaimPaidDate { get => claimPaidDate; set => claimPaidDate = value; }

        // 40I; Service details
        public List<ServiceLineDetails> ServiceLineDetails { get; set; }

        // 431; Payment details
        public List<PlanPayment> PlanPayment { get; set; }
    }
}