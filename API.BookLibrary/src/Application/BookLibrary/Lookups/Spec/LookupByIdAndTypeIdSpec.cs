using Ardalis.Specification;
using Domain.Models;

namespace Application.BookLibrary.Lookups.Dtos.Spec
{
    public class LookupByIdAndTypeIdSpec : Specification<Lookup, LookupDto>
    {
        public LookupByIdAndTypeIdSpec(Guid id, Guid typeId) =>
            Query
            .Where(b => b.Id == id && b.TypeId == typeId)
            .Include(x => x.Type);
    }
}

