using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // הוספת ה-namespace ללוגים
using ProjectChinesesActstion.Services;
using ProjectChinesesActstion.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProjectChinesesActstion.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftServices _GiftServices;
        private readonly ILogger<GiftController> _logger;  // הוספת הלוגר

        // עדכון הקונסטרקטור להוספת הלוגר
        public GiftController(IGiftServices GiftServices, ILogger<GiftController> logger)
        {
            _GiftServices = GiftServices;
            _logger = logger;
        }

        
        [HttpGet("{page}")]
        public async Task<IActionResult> GetByPage( int page)
        {
            _logger.LogInformation("Fetching gifts for page: {Page}", page);  // לוג עבור התחלת שליפת מתנות

            var Gift = await _GiftServices.GetByPage(page);

            _logger.LogInformation("Successfully fetched gifts for page: {Page}", page);  // לוג עבור הצלחה
            return Ok(Gift);
        }
        [Authorize(Roles = "User")]
        [Route("GetByName")]
        [HttpGet]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            _logger.LogInformation("Fetching gifts by name: {Name}", name);  // לוג עבור חיפוש מתנות לפי שם

            var Gift = await _GiftServices.GetByName(name);

            if (Gift != null)
            {
                _logger.LogInformation("Successfully fetched gifts by name: {Name}", name);  // לוג עבור הצלחה
                return Ok(Gift);
            }

            _logger.LogWarning("No gifts found with name: {Name}", name);  // לוג אזהרה אם לא נמצאו מתנות
            return NotFound($"No gifts found with name: {name}");
        }
        [Authorize(Roles = "User")]
        [Route("GetByDonator")]
        [HttpGet]
        public async Task<IActionResult> GetByDonator([FromQuery] string name)
        {
            _logger.LogInformation("Fetching gifts by donator: {DonatorName}", name);  // לוג עבור חיפוש מתנות לפי שם תורם

            var Gift = await _GiftServices.GetByDonator(name);

            if (Gift != null)
            {
                _logger.LogInformation("Successfully fetched gifts by donator: {DonatorName}", name);  // לוג עבור הצלחה
                return Ok(Gift);
            }

            _logger.LogWarning("No gifts found for donator: {DonatorName}", name);  // לוג אזהרה אם לא נמצאו מתנות
            return NotFound($"No gifts found for donator: {name}");
        }
        [Authorize(Roles = "User")]
        [Route("GetCheapGift")]
        [HttpGet]
        public async Task<IActionResult> GetCheapGift([FromQuery] int page)
        {
            _logger.LogInformation("Fetching cheap gifts for page: {Page}", page);  // לוג עבור חיפוש מתנות זולות

            var Gift = await _GiftServices.GetCheapGift(page);

            _logger.LogInformation("Successfully fetched cheap gifts for page: {Page}", page);  // לוג עבור הצלחה
            return Ok(Gift);
        }
        [Authorize(Roles = "User")]
        [Route("GetExpensiveGift")]
        [HttpGet]
        public async Task<IActionResult> GetExpensiveGift([FromQuery] int page)
        {
            _logger.LogInformation("Fetching expensive gifts for page: {Page}", page);  // לוג עבור חיפוש מתנות יקרות

            var Gift = await _GiftServices.GetExpensiveGift(page);

            _logger.LogInformation("Successfully fetched expensive gifts for page: {Page}", page);  // לוג עבור הצלחה
            return Ok(Gift);
        }
        [Authorize(Roles = "User")]
        [Route("GetByCategory")]
        [HttpGet]
        public async Task<IActionResult> GetByCategory([FromQuery] int page)
        {
            _logger.LogInformation("Fetching gifts by category for page: {Page}", page);  // לוג עבור חיפוש מתנות לפי קטגוריה

            var Gift = await _GiftServices.GetByCategory(page);

            _logger.LogInformation("Successfully fetched gifts by category for page: {Page}", page);  // לוג עבור הצלחה
            return Ok(Gift);
        }
        [Authorize(Roles = "Admin")]
        [Route("GetNumCustomer")]
        [HttpGet]
        public int GetNumCustomer( [FromBody] string name)
        {
            _logger.LogInformation("Fetching number of customers for gift: {GiftName}", name);  // לוג עבור חיפוש מספר לקוחות

            var numust = _GiftServices.GetNumCustomer(name);

            _logger.LogInformation("Successfully fetched number of customers for gift: {GiftName}", name);  // לוג עבור הצלחה
            return numust;
        }

       
        [Route("GetCategory")]
        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            _logger.LogInformation("Fetching all category"); 

            var category = await _GiftServices.GetCategory();

            _logger.LogInformation("Successfully fetched all category");  // לוג עבור הצלחה
            return Ok(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add([FromBody] Gift gift)
        {
            _logger.LogInformation("Attempting to add new gift with Id: {GiftId}", gift.Id);  // לוג עבור התחלת הוספת מתנה

            var success = _GiftServices.Add(gift);
            if (success)
            {
                _logger.LogInformation("Successfully added gift with Id: {GiftId}", gift.Id);  // לוג עבור הצלחה
                return Ok(new { message = $"The gift {gift.Id} added successfully" });
            }

            _logger.LogWarning("Failed to add gift with Id: {GiftId}", gift.Id);  // לוג אזהרה אם לא הצלחנו להוסיף את המתנה
            return BadRequest(new { errors = new { General = new[] { $"The gift {gift.Id} not successfully added" } } });
            
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult Update([FromBody] Gift gift)
        {
            _logger.LogInformation("Attempting to update gift with Id: {GiftId}", gift.Id);  // לוג עבור התחלת עדכון מתנה

            bool succss = _GiftServices.Update(gift);
            if (succss) { 
             _logger.LogInformation("Successfully updated gift with Id: {GiftId}", gift.Id);  // לוג עבור הצלחה
                return Ok(new { message = $"Successfully updated gift {gift.Name}" });
            }
            _logger.LogInformation("Not Successfully updated gift with Id: {GiftId}", gift.Id);  // לוג עבור הצלחה
            return BadRequest(new { errors = new { General = new[] { $"The  {gift.Name} not successfully updated " } } });

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Attempting to delete gift with Id: {GiftId}", id);  // לוג עבור התחלת מחיקת מתנה

            _GiftServices.Delete(id);

            _logger.LogInformation("Successfully deleted gift with Id: {GiftId}", id);  // לוג עבור הצלחה
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [Route("DeleteMany")]
        [HttpDelete]
        public IActionResult DeleteMany([FromQuery] int[] id)
        {
            _logger.LogInformation("Attempting to delete many gifts with Ids: {GiftIds}", string.Join(", ", id));  // לוג עבור התחלת מחיקת מתנות מרובות

            _GiftServices.DeleteMany(id);

            _logger.LogInformation("Successfully deleted many gifts with Ids: {GiftIds}", string.Join(", ", id));  // לוג עבור הצלחה
            return NoContent();
        }
    }
}
