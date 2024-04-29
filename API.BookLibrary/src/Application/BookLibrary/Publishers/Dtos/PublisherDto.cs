using Domain.Common.Contracts;

namespace Application.BookLibrary.Publishers.Dtos
{
    public class PublisherDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
