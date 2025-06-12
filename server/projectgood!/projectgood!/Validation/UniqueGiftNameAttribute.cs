using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Validation
{
    public class UniqueGiftNameAttribute : ValidationAttribute
    {
        private string connectionString = "data source=srv2\\pupils;initial catalog=ProjectChani&Ayala;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";

        // מתודולוגיה לשימוש בהקשר כדי לזהות אם מדובר בעדכון
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string name = value as string;
            var currentGift = validationContext.ObjectInstance as Gift;

            // אם שם המתנה ריק, לא נבצע בדיקה
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }

            // אם מדובר בעדכון, והשם לא השתנה, אז לא נבצע את הבדיקה
            if (currentGift != null && currentGift.Id > 0 && currentGift.Name == name)
            {
                return ValidationResult.Success; // אם השם לא השתנה, לא נבצע בדיקה
            }

            // אם השם כן השתנה, נבדוק אם הוא קיים במערכת
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM [Gift] WHERE Name = @Name";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            return new ValidationResult("שם המתנה כבר קיים במערכת.");
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

            return ValidationResult.Success; // אם לא קיים במערכת
        }
    }
}
