using Application.BookLibrary.LookupTypes.Dto;
using Ardalis.Specification;
using Domain.Models;

namespace Application.BookLibrary.LookupTypes.Spec
{
    public class LookTypeByNameSpec : Specification<LookupType, LookupTypeDto>
    {
        public LookTypeByNameSpec(string name) =>
            Query
            .Where(b => b.Name == name);
    }
}