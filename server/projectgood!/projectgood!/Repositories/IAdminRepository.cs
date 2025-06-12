using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Repositories
{
    public interface IAdminRepository
    {
        public IEnumerable<Admin> Get();
    }
}
