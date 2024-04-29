using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Lookups.Dtos.Requests
{
    public class GetLookupsByTypeIdRequest : IRequest<List<LookupDto>>
    {
        public Guid LookupTypeId { get; set; }

        public GetLookupsByTypeIdRequest(Guid id)
        {
            LookupTypeId = id;
        }
    }

    public class GetLookupsByTypeIdRequestHandler : IRequestHandler<GetLookupsByTypeIdRequest, List<LookupDto>>
    {
        private readonly IReadRepository<Lookup> _repository;

        public GetLookupsByTypeIdRequestHandler(IReadRepository<Lookup> repository) =>
            (_repository) = (repository);

        public async Task<List<LookupDto>> Handle(GetLookupsByTypeIdRequest request, CancellationToken cancellationToken)
        {
            List<LookupDto>? lookups = await _repository.ListAsync(new LookupByTypeIdSpec(request.LookupTypeId), cancellationToken);
            return lookups;
        }
    }
}