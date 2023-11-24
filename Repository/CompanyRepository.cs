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

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
            await GetAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) => 
            await GetByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

        public async Task CreateCompanyAsync(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await GetByCondition(c => ids.Contains(c.Id), trackChanges).ToListAsync();

        public void DeleteCompany(Company company) => Delete(company);
    }
}
