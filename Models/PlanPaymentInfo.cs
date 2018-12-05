using System;

namespace ClaimsApi.Models {
    public class PlanPaymentInfo {
        public string PolicyNumber { get; set; }
        public string SubscriberRelationship { get; set; }

        private DateTime claimPaidDate;

        public DateTime GetClaimPaidDate()
        {
            return claimPaidDate;
        }

        public void SetClaimPaidDate(DateTime value) => claimPaidDate = value;
    }
}