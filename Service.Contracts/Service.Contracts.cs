using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompanies(bool trackChanges);
        Task<CompanyDTO> GetCompany(Guid companyId, bool trackChanges);
        Task<CompanyDTO> CreateCompany(CompanyForCreationDTO company);
        Task<IEnumerable<CompanyDTO>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDTO> companyCollection);
    }

    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetEmployees(Guid companyId, bool trackChanges);
        Task<EmployeeDTO> GetEmployee(Guid id, Guid companyId, bool trackChanges);
        Task<EmployeeDTO> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDTO employee, bool trackChanges);
        Task DeleteEmployeeForCompany(Guid id, Guid companyId, bool trackChanges);
    }

    public interface IServiceManager
    {
        ICompanyService CompanyService { get; }
        IEmployeeService EmployeeService { get; }
    }
}