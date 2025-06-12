using System.Security.Cryptography;
using System.Text;
using Effortless.Net.Encryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectChinesesActstion.Models;
using ProjectChinesesActstion.Repositories;
using ProjectChinesesActstion.Services;
using Serilog;

namespace ProjectChinesesActstion.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly ProjectContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IAdminRepository _AdminRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private static readonly Serilog.ILogger _logger = Log.ForContext<AuthServices>(); // Create a logger instance

        // Secret key and IV used for encryption
        public AuthServices(ProjectContext context, ICustomerRepository CustomerRepository, JwtTokenService jwtTokenService, IAdminRepository AdminRepository)
        {
            _context = context;
            _CustomerRepository = CustomerRepository;
            _jwtTokenService = jwtTokenService;
            _AdminRepository = AdminRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public Customer Register(CustomerMask user)
        {
            try
            {
                _logger.Information($"Registering user with email: {user.Email}");
                User user1 = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Adress = user.Adress,
                    Phone = user.Phone
                };
                // Add user to database
                _context.User.Add(user1);
                    _context.SaveChanges();

                // Encrypt the password (use _passwordHasher)
                var hashedPassword = _passwordHasher.HashPassword(user1, user.Password); // זה יוצר את הסיסמה המוצפנת
                Customer c = new Customer { UserId = user1.Id, Password = hashedPassword };

                // Add customer to the database
                _context.Customer.Add(c);
                _context.SaveChanges();

                _logger.Information($"User {user.Email} registered successfully.");

                return c;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error registering user {user.Email}");
                return null;
            }
        }

        public string Login(Login request)
        {
            try
            {
                _logger.Information($"Attempting login for email: {request.Email}");

                var userA = _AdminRepository.Get();
                var emailA = userA.FirstOrDefault(x => x.Email == request.Email);

                if (emailA != null)
                {
                    // Verify admin's password
                    if (emailA.Password == request.Password) // בהנחה שאתה לא מצפין את סיסמת המנהל
                    {
                        var roles = new List<string> { "User", "Admin" };
                        var token = _jwtTokenService.GenerateJwtToken(request.Email, roles, emailA.Id);
                        _logger.Information($"Login successful for admin: {request.Email}");
                        return token;
                    }
                    else return "סיסמא שגויה";

                }

                // First check customer repository
                var user = _CustomerRepository.Get();
                var emailc = user.FirstOrDefault(x => x.Email == request.Email);
                User uu = null;

                if (emailc != null)
                {
                    // Get the user from _context
                    uu = _context.User.FirstOrDefault(x => x.Email == emailc.Email);

                    if (uu != null)
                    {
                        var result = _passwordHasher.VerifyHashedPassword(uu, emailc.Password, request.Password);
                        if (result == PasswordVerificationResult.Success)
                        {
                            var roles = new List<string> { "User" };
                            var token = _jwtTokenService.GenerateJwtToken(request.Email, roles, uu.Id); // הוספת UserId לטוקן
                            _logger.Information($"Login successful for customer: {request.Email}");
                            return token;
                        }
                        else return "סיסמא שגויה";
                    }
                }
                _logger.Warning($"Invalid login attempt for email: {request.Email}");
                return "משתמש לא חוקי. לכניסה למערכת בצע הרשמה";

            }

            catch (Exception ex)
            {
                _logger.Error(ex, "Error during login process.");
                return "Invalid credentials";
            }
        }

    }
}
