using Microsoft.EntityFrameworkCore;
using ProjectChinesesActstion.Models;


namespace ProjectChinesesActstion.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ProjectContext _context;
        public AdminRepository(ProjectContext context)
        {
            _context = context;
        }

        public IEnumerable<Admin> Get()
        {
            var Admin = _context.Admin.ToList();

            if (Admin != null)
                return Admin.ToList();
            return null;
        }
    }
}
