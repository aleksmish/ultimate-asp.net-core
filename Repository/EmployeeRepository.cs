using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            await Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);

        public async Task<Employee> GetEmployee(Guid id, Guid companyId, bool trackChanges) =>
            await GetByCondition(e => e.CompanyId == companyId && e.Id == id, trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges) =>
            await GetByCondition(e => e.CompanyId == companyId, trackChanges)
            .OrderBy(e => e.Name).ToListAsync();
    }
}
