using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.ComponentModel.Design;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
        }

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(id);

            return company;
        }

        public async Task<CompanyDTO> CreateCompanyAsync(CompanyForCreationDTO company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            await _repository.Company.CreateCompanyAsync(companyEntity);
            await _repository.SaveAsync();

            var companyDTO = _mapper.Map<CompanyDTO>(companyEntity);

            return companyDTO;
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(bool trackChanges)
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
            var companiesDTOs = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

            return companiesDTOs;
        }

        public async Task<IEnumerable<CompanyDTO>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var companyDTOs = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);

            return companyDTOs;
        }

        public async Task<CompanyDTO> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            var companyDTO = _mapper.Map<CompanyDTO>(company);

            return companyDTO;
        }

        public async Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDTO> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (var company in companyEntities)
            {
                await _repository.Company.CreateCompanyAsync(company);
            }

            await _repository.SaveAsync();

            var companyCollectionDTO = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
            var ids = string.Join(",", companyCollectionDTO.Select(c => c.Id));

            return (companies: companyCollectionDTO, ids: ids);
        }

        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDTO company, bool trackChanges)
        {
            var companyEntity = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();
        }

    }
}