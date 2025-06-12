using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Validation
{
    public class UniqueGiftNameAttribute : ValidationAttribute
    {
        private string connectionString = "data source=srv2\\pupils;initial catalog=ProjectChani&Ayala;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";

        // ���������� ������ ����� ��� ����� �� ����� ������
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string name = value as string;
            var currentGift = validationContext.ObjectInstance as Gift;

            // �� �� ����� ���, �� ���� �����
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }

            // �� ����� ������, ���� �� �����, �� �� ���� �� ������
            if (currentGift != null && currentGift.Id > 0 && currentGift.Name == name)
            {
                return ValidationResult.Success; // �� ��� �� �����, �� ���� �����
            }

            // �� ��� �� �����, ����� �� ��� ���� ������
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
                            return new ValidationResult("�� ����� ��� ���� ������.");
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    return new ValidationResult("����� �������� ���� ������: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    return new ValidationResult("����� �����: " + ex.Message);
                }
            }

            return ValidationResult.Success; // �� �� ���� ������
        }
    }
}
