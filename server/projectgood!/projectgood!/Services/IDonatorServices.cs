using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Services
{
    public interface IDonatorServices
    {
        public Task<IEnumerable<DonatorMask>> Get();
        public Task<IEnumerable<DonatorMask>> GetByPage(int page);
        public bool Add(User user);
        public bool Update(User user);
        public bool Delete(int id);
        public Task<IEnumerable<DonatorMask>> GetByName(string name);
        public Task<DonatorMask> GetByEmail(string email);
        public Task<IEnumerable<DonatorMask>> GetByGift(string gift);
    }
}
