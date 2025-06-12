using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // הוספת ה-namespace ללוגים
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Services;

namespace ProjectChinesesActstion.Controlers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DonatorController : ControllerBase
    {
        private readonly IDonatorServices _DonatorServices;
        private readonly ILogger<DonatorController> _logger;  // הוספת הלוגר

        // עדכון הקונסטרקטור להוספת הלוגר
        public DonatorController(IDonatorServices DonatorServices, ILogger<DonatorController> logger)
        {
            _DonatorServices = DonatorServices;
            _logger = logger;
        }
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching donators");  // לוג עבור התחלת שליפת הנתונים

            var donator = await _DonatorServices.Get();

            _logger.LogInformation("Successfully fetched donators");  // לוג עבור הצלחה
            return Ok(donator);
        }


        [HttpGet("{page}")]
        public async Task<IActionResult> GetByPage(int page)
        {
            _logger.LogInformation("Fetching donators for page: {Page}", page);  // לוג עבור התחלת שליפת הנתונים

            var donator = await _DonatorServices.GetByPage(page);

            _logger.LogInformation("Successfully fetched donators for page: {Page}", page);  // לוג עבור הצלחה
            return Ok(donator);
        }

        [Route("GetByName")]
        [HttpGet]
        public async Task<IActionResult> GetByName(string name)
        {
            _logger.LogInformation("Fetching donators by name: {Name}", name);  // לוג עבור התחלת חיפוש לפי שם

            var donator = await _DonatorServices.GetByName(name);

            if (donator != null)
            {
                _logger.LogInformation("Successfully fetched donators by name: {Name}", name);  // לוג עבור הצלחה
                return Ok(donator);
            }

            _logger.LogWarning("No donators found with name: {Name}", name);  // לוג אזהרה אם לא נמצאו תורמים
            return NotFound($"No donators found with name: {name}");
        }

        [Route("GetByEmail")]
        [HttpGet]
        public async Task<IActionResult> GetByEmail(string email)
        {
            _logger.LogInformation("Fetching donators by email: {Email}", email);  // לוג עבור התחלת חיפוש לפי אימייל

            var donator = await _DonatorServices.GetByEmail(email);

            if (donator != null)
            {
                _logger.LogInformation("Successfully fetched donators by email: {Email}", email);  // לוג עבור הצלחה
                return Ok(donator);
            }

            _logger.LogWarning("No donators found with email: {Email}", email);  // לוג אזהרה אם לא נמצאו תורמים
            return NotFound($"No donators found with email: {email}");
        }

        [Route("GetByGift")]
        [HttpGet]
        public async Task<IActionResult> GetByGift(string gift)
        {
            _logger.LogInformation("Fetching donators by gift: {Gift}", gift);  // לוג עבור התחלת חיפוש לפי מתנה

            var donator = await _DonatorServices.GetByGift(gift);

            if (donator != null)
            {
                _logger.LogInformation("Successfully fetched donators by gift: {Gift}", gift);  // לוג עבור הצלחה
                return Ok(donator);
            }

            _logger.LogWarning("No donators found with gift: {Gift}", gift);  // לוג אזהרה אם לא נמצאו תורמים
            return NotFound($"No donators found with gift: {gift}");
        }

        [HttpPost]
        public IActionResult Add([FromBody] User user)
        {
            _logger.LogInformation("Attempting to add a new donator: {DonatorName}", user.Name);

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            if (!isValid)
            {
                // מחזירים את כל השגיאות בצורה מאורגנת לפי שם שדה
                var errors = validationResults
                    .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "General")
                    .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage).ToArray());

                return BadRequest(new { errors });
            }

            var success = _DonatorServices.Add(user);
            if (success)
            {
                _logger.LogInformation("Successfully added donator: {DonatorName}", user.Name);
                return Ok(new { message = $"The Donator {user.Name} added successfully" });

            }

            _logger.LogWarning("Failed to add donator: {DonatorName}", user.Name);
            return BadRequest(new { errors = new { General = new[] { $"The Donator {user.Name} not successfully added" } } });
        }


        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            _logger.LogInformation("Attempting to update donator with Id: {DonatorId}", user.Name);  // לוג עבור התחלת עדכון תורם

            var sucs = _DonatorServices.Update(user);
            if (sucs) { 
            _logger.LogInformation("Successfully updated donator with Id: {DonatorId}", user.Name);  // לוג עבור הצלחה
                return Ok(new { message = $"Successfully updated donator {user.Name}" });

            }
            else
            {
                return BadRequest(new { errors = new { General = new[] { $"The Donator {user.Name} not successfully updated donator" } } });
               
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Attempting to delete donator with Id: {DonatorId}", id);  // לוג עבור התחלת מחיקת תורם

            var susc =_DonatorServices.Delete(id);
            if (susc)
            {
                _logger.LogInformation("Successfully deleted donator with Id: {DonatorId}", id);  // לוג עבור הצלחה
                return Ok();
            }
            return BadRequest();
        }
    }
}
