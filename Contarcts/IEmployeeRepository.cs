﻿using Entities.Models;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges);
        Task<Employee> GetEmployee(Guid id, Guid companyId, bool trackChanges);
        Task CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
