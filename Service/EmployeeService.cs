using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employees = await _repository.Employee.GetEmployees(companyId, trackChanges);
            var employeesDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            return employeesDTOs;
        }

        public async Task<EmployeeDTO> GetEmployee(Guid id, Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employee = await _repository.Employee.GetEmployee(id, companyId, trackChanges);

            if (employee is null)
                throw new EmployeeNotFoundException(id);

            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

            return employeeDTO;
        }

        public async Task<EmployeeDTO> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDTO employee, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employee);

            await _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeDTO = _mapper.Map<EmployeeDTO>(employeeEntity);

            return employeeDTO;
        }

        public async Task DeleteEmployeeForCompany(Guid id, Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeForCompany = await _repository.Employee.GetEmployee(id, companyId, trackChanges);

            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();
        }
    }
}
