using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompanies(bool trackChanges);
        Task<CompanyDTO> GetCompany(Guid companyId, bool trackChanges);
    }

    public interface IEmployeeService
    {

    }

    public interface IServiceManager
    {
        ICompanyService CompanyService { get; }
        IEmployeeService EmployeeService { get; }
    }
}