using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // הוספת ה-namespace ללוגים
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Services;

namespace ProjectChinesesActstion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _AuthServices;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;  // הוספת הלוגר

        // עדכון הקונסטרקטור להוספת הלוגר
        public AuthController(JwtTokenService jwtTokenService, IAuthServices AuthServices, ILogger<AuthController> logger)
        {
            _jwtTokenService = jwtTokenService;
            _AuthServices = AuthServices;
            _logger = logger;
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            try
            {
                _logger.LogInformation("Login attempt started for user: {Email}", request.Email);

                var token = _AuthServices.Login(request);

                if (token != "משתמש לא חוקי. לכניסה למערכת בצע הרשמה" && token != "סיסמא שגויה")
                {
                    _logger.LogInformation("Login successful for user: {Email}", request.Email);
                    return Ok(new { token });
                }

                else 
                _logger.LogWarning("Login failed for user: {Email}", request.Email);
                return Unauthorized(new { error = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
            }
        }

        [HttpPost]
        public IActionResult Register([FromBody] CustomerMask user)
        {
            try
            {
                _logger.LogInformation("Registration attempt started for user: {UserName}", user.Name);

                // בדיקת ולידציה בצד השרת
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Validation failed for user: {UserName}", user.Name);

                    // חזרה עם שגיאות הוולידציה בפורמט JSON
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new { errors });
                }

                var cus = _AuthServices.Register(user);

                if (cus != null)
                {
                    _logger.LogInformation("Registration successful for user: {UserName}", user.Name);
                    return Ok(new { message = $"The user {user.Id} added successfully" });
                }

                _logger.LogWarning("Registration failed for user: {UserName}", user.Name);
                return BadRequest(new { error = $"The user {user.Id} was not successfully added" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user: {UserName}", user.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred" });
            }
        }

    }
}
