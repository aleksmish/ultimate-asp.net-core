using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            
        }

        public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges) =>
            await GetAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();

        public async Task<Company> GetCompany(Guid companyId, bool trackChanges) => 
            await GetByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

        public async Task CreateCompany(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            await GetByCondition(c => ids.Contains(c.Id), trackChanges).ToListAsync();
    }
}
