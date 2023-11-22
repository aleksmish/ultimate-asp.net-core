using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
        Task<Company> GetCompany(Guid companyId, bool trackChanges);
        Task CreateCompany(Company company);
        Task<IEnumerable<Company>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    }
}
