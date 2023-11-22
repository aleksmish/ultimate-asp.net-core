using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        public async Task<CompanyDTO> CreateCompany(CompanyForCreationDTO company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            await _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyDTO = _mapper.Map<CompanyDTO>(companyEntity);

            return companyDTO;
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies(bool trackChanges)
        {
            var companies = await _repository.Company.GetAllCompanies(trackChanges);
            var companiesDTOs = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

            return companiesDTOs;
        }

        public async Task<IEnumerable<CompanyDTO>> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companyEntities = await _repository.Company.GetByIds(ids, trackChanges);

            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var companyDTOs = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);

            return companyDTOs;
        }

        public async Task<CompanyDTO> GetCompany(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var companyDTO = _mapper.Map<CompanyDTO>(company);

            return companyDTO;
        }

        public async Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDTO> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (var company in companyEntities)
            {
                await _repository.Company.CreateCompany(company);
            }

            await _repository.SaveAsync();

            var companyCollectionDTO = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
            var ids = string.Join(",", companyCollectionDTO.Select(c => c.Id));

            return (companies: companyCollectionDTO, ids: ids);
        }
    }
}