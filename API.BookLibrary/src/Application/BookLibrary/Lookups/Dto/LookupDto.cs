using Application.BookLibrary.LookupTypes.Dto;
using Domain.Common.Contracts;

namespace Application.BookLibrary.Lookups.Dtos
{
    public class LookupDto : BaseEntityDto
    {
        public string Label { get; set; } = default!;

        public Guid TypeId { get; set; }

        public LookupTypeDto? Type { get; set; }
    }
}