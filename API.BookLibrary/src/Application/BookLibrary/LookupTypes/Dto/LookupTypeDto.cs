using Domain.Common.Contracts;

namespace Application.BookLibrary.LookupTypes.Dto
{
    public class LookupTypeDto : BaseEntityDto
    {
        public string Name { get; set; } = default!;
    }
}