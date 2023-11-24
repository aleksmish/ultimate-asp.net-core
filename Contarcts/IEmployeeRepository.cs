using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid id, Guid companyId, bool trackChanges);
        Task CreateEmployeeForCompanyAsync(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
