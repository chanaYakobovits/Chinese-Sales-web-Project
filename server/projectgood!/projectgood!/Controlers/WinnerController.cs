using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // הוספת ה-namespace ללוגים
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Services;

namespace ProjectChinesesActstion.Controlers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class WinnerController : ControllerBase
    {
        private readonly IWinnerServices _WinnerServices;
        private readonly ILogger<WinnerController> _logger;  // הוספת הלוגר

        // עדכון הקונסטרקטור להוספת הלוגר
        public WinnerController(IWinnerServices WinnerServices, ILogger<WinnerController> logger)
        {
            _WinnerServices = WinnerServices;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{giftId}")]
        public async Task<IActionResult> GetByGift(int giftId)
        {
            _logger.LogInformation("Fetching winners for gift with ID: {GiftId}", giftId);  // לוג עבור שליפת זוכים לפי מתנה

            var win = await _WinnerServices.GetByGift(giftId);

            if (win != null)
            {
                _logger.LogInformation("Successfully fetched winners for gift with ID: {GiftId}", giftId);  // לוג עבור הצלחה
                return Ok(win);
            }

            _logger.LogWarning("No winners found for gift with ID: {GiftId}", giftId);  // לוג אזהרה אם לא נמצאו זוכים
            return NotFound($"No winners found for gift with ID: {giftId}");
        }

        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all winners");  // לוג עבור שליפת כל הזוכים

            var win = await _WinnerServices.Get();

            _logger.LogInformation("Successfully fetched all winners");  // לוג עבור הצלחה
            return Ok(win);
        }

        [Authorize(Roles = "Admin")]
        [Route("GetPayment")]
        [HttpGet]
        public IActionResult GetPayment()
        {
            _logger.LogInformation("Fetching payment details for winners");  // לוג עבור שליפת פרטי תשלום לזוכים

            var pay = _WinnerServices.GetPayment();

            _logger.LogInformation("Successfully fetched payment details for winners");  // לוג עבור הצלחה
            return Ok(pay);
        }
    }
}
