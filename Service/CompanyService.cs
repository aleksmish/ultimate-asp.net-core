using AutoMapper;
using Contracts;
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

        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies(bool trackChanges)
        {
            var companies = await _repository.Company.GetAllCompanies(trackChanges);
            var companiesDTOs = _mapper.Map<IEnumerable<CompanyDTO>>(companies);

            return companiesDTOs;
        }

        public async Task<CompanyDTO> GetCompany(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges);
            var companyDTO = _mapper.Map<CompanyDTO>(company);

            return companyDTO;
        }
    }
}