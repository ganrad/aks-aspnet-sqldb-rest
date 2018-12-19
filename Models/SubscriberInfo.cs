using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClaimsApi.Models 
{
    [Table("Subscriber_Info")]
    public class SubscriberInfo
    {
        [Column("subscriber_id")]
        public int SubscriberInfoId { get; set; }
        [Column("subscriber_reln")]
        public string SubscriberRelationship { get; set; }
        [Column("subscriber_policy_no")]
        public string SubscriberPolicyNumber { get; set; }
        [Column("insured_group_name")]
        public string InsuredGroupName { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Column("subscriber_last_name")]
        public string SubscriberLastName { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Column("subscriber_first_name")]
        public string SubscriberFirstName { get; set; }
        [Column("subscriber_middle_name")]
        public string SubscriberMiddleName { get; set; }
        [Required]
        [Column("subscriber_id_ssn")]
        public string SubscriberIdentifierSSN { get; set; }
        [Column("subscriber_addr_1")]
        public string SubscriberAddressLine1 { get; set; }
        [Column("subscriber_address_2")]
        public string SubscriberAddressLine2 { get; set; }
        [Column("subscriber_city")]
        public string SubscriberCity { get; set; }
        [Column("subscriber_state")]
        public string SubscriberState { get; set; }
        [Column("subscriber_postal_code")]
        public string SubscriberPostalCode { get; set; }
        [Column("subscriber_country")]
        public string SubscriberCountry { get; set; }
        [Column("subscriber_dob")]
        public string SubDateOfBirth { get; set; }
        [Column("subscriber_gender")]
        public string SubscriberGender { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Column("payer_name")]
        public string PayerName { get; set; }
        [Column("patient_last_name")]
        public string PatientLastName { get; set; }
        [Column("patient_first_name")]
        public string PatientFirstName { get; set; }
        [Column("patient_ssn")]
        public string PatientSSN { get; set; }
        [Column("patient_member_id")]
        public string PatientMemberID { get; set; }
        [Column("patient_dob")]
        public string PatientDOB { get; set; }
        [Column("patient_gender")]
        public string PatientGender { get; set; }
        [Column("category_of_service")]
        public string CatgOfService { get; set; }
        [Required]
        [Column("claim_id")]
        public int ClaimItemId { get; set; }
    }
    
}