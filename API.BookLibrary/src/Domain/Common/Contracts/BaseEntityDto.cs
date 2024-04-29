using API.AxiomManagementSystem.Domain.Common.Contracts;

namespace Domain.Common.Contracts
{
    public class BaseEntityDto : IEntityDto
    {
        public Guid Id { get; set; } = default!;
    }
}
