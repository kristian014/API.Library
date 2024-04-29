using Domain.Common.Contracts;

namespace Application.BookLibrary.Authors.Dtos
{
    public class AuthorDto : BaseEntityDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
    }
}
