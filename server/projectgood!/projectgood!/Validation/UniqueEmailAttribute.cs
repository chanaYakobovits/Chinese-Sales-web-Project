using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace ProjectChinesesActstion.Validation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        private string connectionString = "data source=srv2\\pupils;initial catalog=ProjectChani&Ayala;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = value as string;

            if (string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success; // אם המייל ריק, לא בודקים אותו
            }

            // זיהוי אם זה עדכון לפי Id
            var idProperty = validationContext.ObjectType.GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(validationContext.ObjectInstance);
                if (idValue != null && idValue is int id && id > 0)
                {
                    // זהו עדכון, לא בודקים את המייל
                    return ValidationResult.Success;
                }
            }

            // בדיקה אם המייל כבר קיים
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            return new ValidationResult("המייל כבר קיים במערכת.");
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    return new ValidationResult("שגיאה בהתחברות למסד נתונים: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    return new ValidationResult("שגיאה כלשהי: " + ex.Message);
                }
            }

            return ValidationResult.Success; // אם המייל לא קיים במערכת
        }
    }
}
