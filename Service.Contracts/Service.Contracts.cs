using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(bool trackChanges);
        Task<CompanyDTO> GetCompanyAsync(Guid companyId, bool trackChanges);
        Task<CompanyDTO> CreateCompanyAsync(CompanyForCreationDTO company);
        Task<IEnumerable<CompanyDTO>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDTO> companyCollection);
        Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
        Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDTO company, bool trackChanges);
    }

    public interface IEmployeeService
    {
        Task<(IEnumerable<EmployeeDTO> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
        Task<EmployeeDTO> GetEmployeeAsync(Guid id, Guid companyId, bool trackChanges);
        Task<EmployeeDTO> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDTO employee, bool trackChanges);
        Task DeleteEmployeeForCompanyAsync(Guid id, Guid companyId, bool trackChanges);
        Task UpdateEmployeeForCompanyAsync(Guid id, Guid companyId, EmployeeForUpdateDTO employee, bool compTrackChanges, bool empTrackChanges);
        Task<(EmployeeForUpdateDTO employeeForPatch, Employee employee)> GetEmployeeForPatchAsync(Guid id, Guid companyId, bool compTrackChanges, bool empTrackChanges);
        Task SaveChangesForPatchAsync(EmployeeForUpdateDTO employeeForUpdate, Employee employee);
    }

    public interface IServiceManager
    {
        ICompanyService CompanyService { get; }
        IEmployeeService EmployeeService { get; }
    }
}