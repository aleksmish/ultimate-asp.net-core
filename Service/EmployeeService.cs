using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid id, Guid companyId, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeAsync(id, companyId, trackChanges);

            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);

            return employeeDb;
        }

        public async Task<(IEnumerable<EmployeeDTO> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            if (!employeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            await CheckIfCompanyExists(companyId, trackChanges);

            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
            var employeesDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employeesWithMetaData);

            return (employees: employeesDTOs, metaData: employeesWithMetaData.MetaData);
        }

        public async Task<EmployeeDTO> GetEmployeeAsync(Guid id, Guid companyId, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employee = GetEmployeeForCompanyAndCheckIfItExists(id, companyId, trackChanges);

            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

            return employeeDTO;
        }

        public async Task<EmployeeDTO> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDTO employee, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employee);

            await _repository.Employee.CreateEmployeeForCompanyAsync(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeDTO = _mapper.Map<EmployeeDTO>(employeeEntity);

            return employeeDTO;
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid id, Guid companyId, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExists(id, companyId, trackChanges);

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid id, Guid companyId, EmployeeForUpdateDTO employee, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(id, companyId, empTrackChanges);

            _mapper.Map(employee, employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDTO employeeForPatch, Employee employee)> GetEmployeeForPatchAsync(Guid id, Guid companyId, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(id, companyId, empTrackChanges);

            var employeeForPatch = _mapper.Map<EmployeeForUpdateDTO>(employeeEntity);

            return (employeeForPatch, employeeEntity);
        }

        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDTO employeeForPatch, Employee employee)
        {
            _mapper.Map(employeeForPatch, employee);

            await _repository.SaveAsync();
        }
    }
}
