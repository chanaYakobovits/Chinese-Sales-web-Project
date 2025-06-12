using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Services
{
    public interface IAuthServices
    {
        public Customer Register(CustomerMask user);
        public string Login(Login request);
    }
}
