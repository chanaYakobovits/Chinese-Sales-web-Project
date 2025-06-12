using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // הוספת ה-namespace ללוגים
using ProjectChinesesActstion.Controllers;
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Services;

namespace ProjectChinesesActstion.Controlers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _CustomerServices;
        private readonly ILogger<CustomerController> _logger;  // הוספת הלוגר

        // עדכון הקונסטרקטור להוספת הלוגר
        public CustomerController(ICustomerServices CustomerServices, ILogger<CustomerController> logger)
        {
            _CustomerServices = CustomerServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching customer data for customerId: {CustomerId}");  // לוג כאשר מתחילים לשלוף את המידע

            try
            {
                int customerId = 0;


                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    _logger.LogWarning("Failed to identify user for order addition.");
                    return Unauthorized("User not authenticated.");
                }

                // המרת ה-UserId למספר שלם (בהנחה שזו הצורה)
                if (int.TryParse(userIdClaim, out int userId))
                {
                    customerId = userId; // ייחוס ה-UserId להזמנה
                }
                else
                {
                    _logger.LogWarning("Invalid UserId format for order addition.");
                    return BadRequest("Invalid UserId.");
                }
                var Gift = await _CustomerServices.Get(customerId);

                _logger.LogInformation("Successfully fetched customer data for customerId: {CustomerId}", customerId);  // לוג עבור הצלחה
                return Ok(Gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching customer data for customerId: {CustomerId}");  // לוג שגיאה במקרה של תקלה
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
