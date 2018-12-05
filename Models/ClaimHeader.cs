namespace ClaimsApi.Models {
    public class ClaimHeader {
        public string ClaimStatus { get; set; }
        public string ClaimType { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string OriginatorID { get; set; }
        public string DestinationID { get; set; }
        public string ClaimInputMethod { get; set; }
    }
}