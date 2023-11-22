namespace Shared.DataTransferObjects
{
    public record CompanyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
    };
}
