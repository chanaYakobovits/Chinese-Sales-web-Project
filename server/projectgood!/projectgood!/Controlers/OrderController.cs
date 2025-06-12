using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // הוספת ה-namespace ללוגים
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Services;

namespace ProjectChinesesActstion.Controlers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _OrderServices;
        private readonly ILogger<OrderController> _logger;  // הוספת הלוגר
        private readonly IConfiguration _configuration;
        // עדכון הקונסטרקטור להוספת הלוגר
        public OrderController(IOrderServices OrderServices, ILogger<OrderController> logger, IConfiguration configuration)
        {
            _OrderServices = OrderServices;
            _logger = logger;
            _configuration = configuration;
        }
        [Authorize(Roles = "Admin")]
        [Route("GetByGift")]
        [HttpGet]
        public IActionResult GetByGift(int giftId)
        {
            _logger.LogInformation("Fetching orders for gift with ID: {GiftId}", giftId);  // לוג עבור שליפת הזמנות לפי מתנה

            var Order = _OrderServices.GetByGift(giftId);

            if (Order != null)
            {
                _logger.LogInformation("Successfully fetched orders for gift with ID: {GiftId}", giftId);  // לוג עבור הצלחה
                return Ok(Order);
            }

            _logger.LogWarning("No orders found for gift with ID: {GiftId}", giftId);  // לוג אזהרה אם לא נמצאו הזמנות
            return NotFound($"No orders found for gift with ID: {giftId}");
        }
        [Authorize(Roles = "User")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int page)
        {
            _logger.LogInformation("Fetching orders for page: {Page}", page);  // לוג עבור שליפת הזמנות

            var Order = await _OrderServices.Get(page);

            _logger.LogInformation("Successfully fetched orders for page: {Page}", page);  // לוג עבור הצלחה
            return Ok(Order);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Add([FromBody] Order order)
        {
            _logger.LogInformation("Attempting to add new order with ID: {OrderId}", order.Id);

            // שליפת UserId מתוך ה-Claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogWarning("Failed to identify user for order addition.");
                return Unauthorized("User not authenticated.");
            }

            // המרת ה-UserId למספר שלם (בהנחה שזו הצורה)
            if (int.TryParse(userIdClaim, out int userId))
            {

                order.CustomerId = userId; // ייחוס ה-UserId להזמנה
            }
            else
            {
                _logger.LogWarning("Invalid UserId format for order addition.");
                return BadRequest("Invalid UserId.");
            }

            var success = _OrderServices.Add(order);

            if (success)
            {
                _logger.LogInformation("Successfully added order with ID: {OrderId}", order.Id);
                return Ok(new { message = $"The order {order.Id} added successfully" });
            }

            _logger.LogWarning("Failed to add order with ID: {OrderId}", order.Id);
            return BadRequest($"The order {order.Id} not successfully added");
        }
        [Authorize(Roles = "User")]
        [Route("Buy")]
        [HttpGet]
        public IActionResult Buy()
        {
            int user = 0;
            _logger.LogInformation("Attempting to buy for order with ID: {userId}");

            // שליפת UserId מתוך ה-Claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogWarning("Failed to identify user for order addition.");
                return Unauthorized("User not authenticated.");
            }

            // המרת ה-UserId למספר שלם (בהנחה שזו הצורה)
            if (int.TryParse(userIdClaim, out int userId))
            {

                user = userId; // ייחוס ה-UserId להזמנה
            }
            else
            {
                _logger.LogWarning("Invalid UserId format for order addition.");
                return BadRequest("Invalid UserId.");
            }

            var success = _OrderServices.Buy(user);

            if (success)
            {
                _logger.LogInformation("Successfully payment order with ID: {userId}",user);
                return Ok($"The user pay {user} successfully");
            }

            _logger.LogWarning("Failed to pay order with ID: {userId}", user);
            return BadRequest($"The payment {user} not successfully");
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            _logger.LogInformation("Attempting to delete order with : {name}", name);  // לוג עבור התחלת מחיקת הזמנה
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
            _OrderServices.Delete(name, customerId);

            _logger.LogInformation("Successfully deleted order with : {name}", name);  // לוג עבור הצלחה
            return NoContent();
        }
    }
}
