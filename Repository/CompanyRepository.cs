using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
    }
}
