using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ClaimsApi.Models;
using System;

namespace ClaimsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly ClaimsContext _context;

        public ClaimsController(ClaimsContext context)
        {
            _context = context;

            if (_context.ClaimItems.Count() == 0)
            {
                // Create a new ClaimItem if collection is empty,
                // which means you can't delete all ClaimItems.
                _context.ClaimItems.Add(createTestClaimData());
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<ClaimItem>> GetAll()
        {
            Console.WriteLine("GetAll() -");
            return _context.ClaimItems.ToList();
        }

        [HttpGet("{id}", Name = "GetClaim")]
        public ActionResult<ClaimItem> GetById(long id)
        {
            Console.WriteLine("GetById() - id=" + id);
            var item = _context.ClaimItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        private ClaimItem createTestClaimData() {
            ClaimItem ciObj = new ClaimItem();

            ciObj.ClaimStatus = "02"; // Claim status code for claim processing system
            ciObj.ClaimType = "InstClaim"; // Institutional claim
            ciObj.SenderID = "CLPCSVNTEST2"; // Claim processor partner value
            ciObj.ReceiverID = "APPCSVNTEST1"; // Application partner value
            ciObj.OriginatorID = "ORGNCSVTEST1"; // Originator partner value (optional)
            ciObj.DestinationID = "DESMEDSTEST1"; // Destination ID for encounters
            ciObj.ClaimInputMethod = "E"; // Claim input method

            ciObj.SubscriberRelationship = "18";
            ciObj.SubscriberPolicyNumber = "12345";
            ciObj.InsuredGroupName = "MD000004";
            ciObj.SubscriberLastName = "Doe";
            ciObj.SubscriberFirstName = "John";
            ciObj.SubscriberMiddleName = "";
            ciObj.SubscriberIdentifierSSN = "489-88-7001";
            ciObj.SubscriberAddressLine1 = "5589 Hawthorne Way";
            ciObj.SubscriberAddressLine2 = "";
            ciObj.SubscriberCity = "Sacramento";
            ciObj.SubscriberState = "CA";
            ciObj.SubscriberPostalCode = "95835";
            ciObj.SubscriberCountry = "US";
            ciObj.SubDateOfBirth = "12-19-1984";
            ciObj.SubscriberGender = "Male";
            ciObj.PayerName = "";
            ciObj.PatientFirstName = "";
            ciObj.PatientLastName = "";
            ciObj.PatientDOB = "12-19-1984";
            ciObj.PatientGender = "Male";
            ciObj.PatientSSN = "489-88-7001";
            ciObj.PatientMemberID = "12345";
            ciObj.CatgOfService = "Consultation";

            ciObj.ClaimNumber = "1234121235";
            ciObj.TotalClaimCharge = 1234.50m;
            ciObj.PatientStatus = "01";
            ciObj.PatientAmountDue = 0m;
            ciObj.ServiceDate = new System.DateTime(); // DateTime.Now; DateTime.Today; new DateTime(2018,10,31,7,0,0);

            ciObj.PolicyNumber = "898435";
            ciObj.SubscriberRelationship = "18";
            ciObj.SetClaimPaidDate(DateTime.Now);

            ciObj.StatementDate = new DateTime(2018,10,31,8,30,0);
            ciObj.LineCounter = 1;
            ciObj.ServiceCodeDescription = "INPT";
            ciObj.LineChargeAmount = 15000.00m;
            ciObj.DrugCode = "UN";
            ciObj.DrugUnitQuantity = 23;
            ciObj.PharmacyPrescriptionNumber = "123897";
            ciObj.ServiceType = "Consultation";
            ciObj.ProviderCode = "72";
            ciObj.ProviderIdentification = "20120904-20120907";
            ciObj.ProviderLastName = "Longhorn";
            ciObj.ProviderFirstName = "Dr. James";
            ciObj.InNetworkIndicator = true;

            ciObj.PrimaryPayerID = "MEDICAID";
            ciObj.CobServicePaidAmount = 15000m;
            ciObj.ServiceCode = "ABC";
            ciObj.PaymentDate = DateTime.Today;
            ciObj.ClaimAdjGroupCode = "HIPAA";
            ciObj.ClaimAdjReasonCode = "CO";
            ciObj.ClaimAdjQuantity ="3";
            ciObj.ClaimAdjAmount = 500.00m;

            return(ciObj);
        }
        /** private ClaimItem createTestClaimData() {
            ClaimItem ciObj = new ClaimItem();

            ClaimHeader ciObj = new ClaimHeader();
            ciObj.ClaimStatus = "02"; // Claim status code for claim processing system
            ciObj.ClaimType = "InstClaim"; // Institutional claim
            ciObj.SenderID = "CLPCSVNTEST2"; // Claim processor partner value
            ciObj.ReceiverID = "APPCSVNTEST1"; // Application partner value
            ciObj.OriginatorID = "ORGNCSVTEST1"; // Originator partner value (optional)
            ciObj.DestinationID = "DESMEDSTEST1"; // Destination ID for encounters
            ciObj.ClaimInputMethod = "E"; // Claim input method
            ciObj.ClaimHeader = ciObj;

            SubscriberInfo ciObj = new SubscriberInfo();
            ciObj.SubscriberRelationship = "18";
            ciObj.SubscriberPolicyNumber = "12345";
            ciObj.InsuredGroupName = "MD000004";
            ciObj.SubscriberLastName = "Doe";
            ciObj.SubscriberFirstName = "John";
            ciObj.SubscriberMiddleName = "";
            ciObj.SubscriberIdentifierSSN = "489-88-7001";
            ciObj.SubscriberAddressLine1 = "5589 Hawthorne Way";
            ciObj.SubscriberAddressLine2 = "";
            ciObj.SubscriberCity = "Sacramento";
            ciObj.SubscriberState = "CA";
            ciObj.SubscriberPostalCode = "95835";
            ciObj.SubscriberCountry = "US";
            ciObj.SubDateOfBirth = "12-19-1984";
            ciObj.SubscriberGender = "Male";
            ciObj.PayerName = "";
            ciObj.PatientFirstName = "";
            ciObj.PatientLastName = "";
            ciObj.PatientDOB = "12-19-1984";
            ciObj.PatientGender = "Male";
            ciObj.PatientSSN = "489-88-7001";
            ciObj.PatientMemberID = "12345";
            ciObj.CatgOfService = "Consultation";
            ciObj.SubscriberInfo = ciObj;

            InstitutionalRecord irObj = new InstitutionalRecord();
            irObj.ClaimNumber = "1234121235";
            irObj.TotalClaimCharge = 1234.50m;
            irObj.PatientStatus = "01";
            irObj.PatientAmountDue = 0m;
            irObj.ServiceDate = new System.DateTime(); // DateTime.Now; DateTime.Today; new DateTime(2018,10,31,7,0,0);
            ciObj.InstitutionalRecord = irObj;

            PlanPaymentInfo ppiObj = new PlanPaymentInfo();
            ppiObj.PolicyNumber = "898435";
            ppiObj.SubscriberRelationship = "18";
            ppiObj.SetClaimPaidDate(DateTime.Now);
            ciObj.PlanPaymentInfo = ppiObj;

            ServiceLineDetails sldObj = new ServiceLineDetails();
            sldObj.StatementDate = new DateTime(2018,10,31,8,30,0);
            sldObj.LineCounter = 1;
            sldObj.ServiceCodeDescription = "INPT";
            sldObj.LineChargeAmount = 15000.00m;
            sldObj.DrugCode = "UN";
            sldObj.DrugUnitQuantity = 23;
            sldObj.PharmacyPrescriptionNumber = "123897";
            sldObj.ServiceType = "Consultation";
            sldObj.ProviderCode = "72";
            sldObj.ProviderIdentification = "20120904-20120907";
            sldObj.ProviderLastName = "Longhorn";
            sldObj.ProviderFirstName = "Dr. James";
            sldObj.InNetworkIndicator = true;
            ciObj.ServiceLineDetails = sldObj;

            PlanPayment ppObj = new PlanPayment();
            ppObj.PrimaryPayerID = "MEDICAID";
            ppObj.CobServicePaidAmount = 15000m;
            ppObj.ServiceCode = "ABC";
            ppObj.PaymentDate = DateTime.Today;
            ppObj.ClaimAdjGroupCode = "HIPAA";
            ppObj.ClaimAdjReasonCode = "CO";
            ppObj.ClaimAdjQuantity ="3";
            ppObj.ClaimAdjAmount = 500.00m;
            ciObj.PlanPayment = ppObj;

            return(ciObj);
        }
        */
    }
}