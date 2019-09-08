using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using ClaimsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/**
* Author : Ganesh Radhakrishnan, garadha (Microsoft)
* Email : ganrad01@gmail.com
* Dated : 11-18-2018
* Description:
* Controller for the Claims API microservice
*
* Notes:
* ID09082019: garadha: Added method for readiness and liveness probes.
*/

namespace ClaimsApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClaimsController : Controller
    {
        private readonly ClaimsContext _context;
        private readonly ILogger _logger;

        public ClaimsController(ClaimsContext context, ILogger<ClaimsController> logger)
        {
            _context = context;
            _logger = logger;

            if (_context.ClaimItems.Count() == 0)
            {
                // Create a new ClaimItem if collection is empty,
                // which means you can't delete all ClaimItems.
                _context.ClaimItems.Add(createTestClaimData());
                _context.SaveChanges();
            }
        }

        // ID09082019.sn
        // GET: api/v1/claims/healthz
	// when this method is called, dotnet runtime automagically does the backend (sql db) check!

	[HttpGet("healthz")]
        public JsonResult checkHealth()
	{
	    _logger.LogInformation("checkHealth() called ....");

	    Dictionary<string, string> health = new Dictionary<string,string>();
	    health.Add("api","api/v1/claims");
	    // perform checks on the api's 
	    health.Add("apiCheck","OK");
	    // check connectivity to backend (SQL server db)
	    health.Add("backendCheck","Ok");

	    return Json(health);
	}
        // ID09082019.en

        // GET: api/v1/claims

        /// <summary>
        /// Get a list of all medical claims records from the backend data store
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ClaimItem>>> GetAllClaims()
        {
            _logger.LogInformation("GetAllClaims()");
	    var localIp = await getPodIpAddress();
	    _logger.LogInformation($"Pod IP Address: {localIp}");
            Request.HttpContext.Response.Headers.Add("X-Pod-IpAddr",localIp);
            return _context.ClaimItems.
                Include(claimItem => claimItem.SubscriberInfo).
                Include(claimItem => claimItem.ServiceLineDetails).
                Include(claimItem => claimItem.PlanPayment).ToList();
        }

	private async Task<String> getPodIpAddress() {
            var ips = await System.Net.Dns.GetHostAddressesAsync(System.Net.Dns.GetHostName());
            var ipAddrString = "";
            foreach (System.Net.IPAddress ip in ips)
                ipAddrString += ip.ToString();
            return ipAddrString;
        }

        // GET: api/v1/claims/{id}

        /// <summary>
        /// Get a medical claims record by 'ID' from the backend data store
        /// </summary>
        /// <param name="id">ID of the claim record in the claims table</param>
        [HttpGet("{id}", Name = "GetClaimById")]
        public ActionResult<ClaimItem> GetClaimById(int id)
        {
            _logger.LogInformation($"GetClaimById() - id={id}");
            var item = _context.ClaimItems.Find(id);
            if (item == null)
            {
                _logger.LogInformation($"GetClaimById() - id={id}, not found");
                return NotFound();
            };
            _context.Entry(item).Collection(clmInfo => clmInfo.SubscriberInfo).Load();
            _context.Entry(item).Collection(clmInfo => clmInfo.ServiceLineDetails).Load();
            _context.Entry(item).Collection(clmInfo => clmInfo.PlanPayment).Load();

            return item;
        }

        // GET: api/v1/claims/fetch?claimno=number

        /// <summary>
        /// Get a medical claims record by 'Claim Number' from the backend data store
        /// </summary>
        /// <param name="claimno">Claim number of the record in the claims table</param>
        [Route("fetch")]
        [HttpGet]
        public ActionResult<ClaimItem> GetByClaimNumber(
            [FromQuery] string claimno)
        {
            _logger.LogInformation($"GetByClaimNumber() - id={claimno}");
            var item = _context.ClaimItems.SingleOrDefault(clmItem => clmItem.ClaimNumber == $"{claimno}");

            if ( item == null )
            {
                _logger.LogInformation($"GetByClaimNumber() - id={claimno}, not found");
                return NotFound();
            };
            
            _context.Entry(item).Collection(clmInfo => clmInfo.SubscriberInfo).Load();
            _context.Entry(item).Collection(clmInfo => clmInfo.ServiceLineDetails).Load();
            _context.Entry(item).Collection(clmInfo => clmInfo.PlanPayment).Load();

            return item;
        }

        // POST: api/v1/claims

        /// <summary>
        /// Post (Insert) a medical claim record in the backend data store
        /// </summary>
        /// <param name="claimItem">Claim record in JSON format to be inserted into the claims table</param>
        [HttpPost]
        public async Task<ActionResult<ClaimItem>> PostClaimItem(ClaimItem claimItem)
        {
            _logger.LogInformation("PostClaimItem()");
            _context.ClaimItems.Add(claimItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimById", new { id = claimItem.ClaimItemId }, claimItem);
        }

        // PUT: api/v1/claims/5

        /// <summary>
        /// Update a medical claim record in the backend data store
        /// </summary>
        /// <param name="id">ID of the claim record in the claims table</param>
        /// <param name="claimItem">Claim record in JSON format to be updated in the claims table</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaimItem(int id, ClaimItem claimItem)
        {
            _logger.LogInformation($"PutClaimItem() - id={id}");
            if (id != claimItem.ClaimItemId)
            {
                _logger.LogInformation($"PutClaimItem() - id={id}, not found");
                return BadRequest();
            }
           
            _context.Entry(claimItem).State = EntityState.Modified;
            foreach (var sub in claimItem.SubscriberInfo) {
                sub.ClaimItemId = id;
                _context.Entry(sub).State = EntityState.Modified;
            }
            foreach (var svc in claimItem.ServiceLineDetails) {
                svc.ClaimItemId = id;
                _context.Entry(svc).State = EntityState.Modified;
            }
            foreach (var pay in claimItem.PlanPayment) {
                pay.ClaimItemId = id;
                _context.Entry(pay).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/v1/claims/5

        /// <summary>
        /// Delete a medical claims record by 'ID' from the backend data store
        /// </summary>
        /// <param name="id">ID of the claim record in the claims table</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClaimItem>> DeleteClaimItem(int id)
        {
            _logger.LogInformation($"DeleteClaimItem() - id={id}");
            var claimItem = await _context.ClaimItems.FindAsync(id);
            if (claimItem == null)
            {
                _logger.LogInformation($"DeleteClaimItem() - id={id}, not found");
                return NotFound();
            }

            _context.ClaimItems.Remove(claimItem);
            await _context.SaveChangesAsync();

            return claimItem;
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

            SubscriberInfo siObj = new SubscriberInfo();
            siObj.SubscriberRelationship = "18";
            siObj.SubscriberPolicyNumber = "12345";
            siObj.InsuredGroupName = "MD000004";
            siObj.SubscriberLastName = "Doe";
            siObj.SubscriberFirstName = "John";
            siObj.SubscriberMiddleName = "";
            siObj.SubscriberIdentifierSSN = "489-88-7001";
            siObj.SubscriberAddressLine1 = "5589 Hawthorne Way";
            siObj.SubscriberAddressLine2 = "";
            siObj.SubscriberCity = "Sacramento";
            siObj.SubscriberState = "CA";
            siObj.SubscriberPostalCode = "95835";
            siObj.SubscriberCountry = "US";
            siObj.SubDateOfBirth = "12-19-1984";
            siObj.SubscriberGender = "Male";
            siObj.PayerName = "";
            siObj.PatientFirstName = "";
            siObj.PatientLastName = "";
            siObj.PatientDOB = "12-19-1984";
            siObj.PatientGender = "Male";
            siObj.PatientSSN = "489-88-7001";
            siObj.PatientMemberID = "12345";
            siObj.CatgOfService = "Consultation";
            List<SubscriberInfo> siList = new List<SubscriberInfo>();
            siList.Add(siObj);
            ciObj.SubscriberInfo = siList;

            ciObj.ClaimNumber = "1234121235";
            ciObj.TotalClaimCharge = 1234.50m;
            ciObj.PatientStatus = "01";
            ciObj.PatientAmountDue = 0m;
            ciObj.ServiceDate = new System.DateTime(); // DateTime.Now; DateTime.Today; new DateTime(2018,10,31,7,0,0);

            ciObj.PolicyNumber = "898435";
            ciObj.ClaimPaidDate = DateTime.Now;

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
            List<ServiceLineDetails> sldList = new List<ServiceLineDetails>();
            sldList.Add(sldObj);
            ciObj.ServiceLineDetails = sldList;

            PlanPayment ppObj = new PlanPayment();
            ppObj.PrimaryPayerID = "MEDICAID";
            ppObj.CobServicePaidAmount = 15000m;
            ppObj.ServiceCode = "ABC";
            ppObj.PaymentDate = DateTime.Today;
            ppObj.ClaimAdjGroupCode = "HIPAA";
            ppObj.ClaimAdjReasonCode = "CO";
            ppObj.ClaimAdjQuantity ="3";
            ppObj.ClaimAdjAmount = 500.00m;
            List<PlanPayment> ppList = new List<PlanPayment>();
            ppList.Add(ppObj);
            ciObj.PlanPayment = ppList;

            return(ciObj);
        }
    }
}
