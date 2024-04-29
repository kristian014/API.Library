using Ardalis.Specification;
using Domain.Models;

namespace Application.BookLibrary.Lookups.Dtos.Spec
{
    public class LookupByTypeIdSpec : Specification<Lookup, LookupDto>
    {
        public LookupByTypeIdSpec(Guid lookupTypeId) =>
            Query
            .Where(p => p.TypeId == lookupTypeId)
            .Include(x => x.Type);
    }
}