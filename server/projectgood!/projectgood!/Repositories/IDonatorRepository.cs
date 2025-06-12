using ProjectChinesesActstion.Models;

namespace ProjectChinesesActstion.Repositories
{
    public interface IDonatorRepository
    {
        public IEnumerable<DonatorMask> Getall();
        public Task<IEnumerable<DonatorMask>> Get();
        public Task<IEnumerable<DonatorMask>> GetByPage(int page);
        public bool Add(User user);
        public void Update(User user);
        public bool Delete(DonatorMask Donator);
        public Task<IEnumerable<DonatorMask>> GetByName(string name);
        public Task<DonatorMask> GetByEmail(string email);
        public Task<IEnumerable<DonatorMask>> GetByGift(string gift);
    }
}
