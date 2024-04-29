using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Lookups.Dtos.Requests
{
    public class GetLookupByIdAndTypeIdRequest : IRequest<LookupDto>
    {
        public Guid LookupId { get; set; }

        public Guid TypeId { get; set; }

        public GetLookupByIdAndTypeIdRequest(Guid id, Guid typeId)
        {
            LookupId = id;
            TypeId = typeId;
        }
    }

    public class GetLookupByIdAndTypeIdHandler : IRequestHandler<GetLookupByIdAndTypeIdRequest, LookupDto>
    {
        private readonly IReadRepository<Lookup> _repository;

        public GetLookupByIdAndTypeIdHandler(IReadRepository<Lookup> repository) =>
            (_repository) = (repository);

        public async Task<LookupDto> Handle(GetLookupByIdAndTypeIdRequest request, CancellationToken cancellationToken)
        {
            LookupDto? lookupDto = await _repository.FirstOrDefaultAsync(new LookupByIdAndTypeIdSpec(request.LookupId, request.TypeId), cancellationToken);
            _ = lookupDto ?? throw new Exception(string.Format("lookup.notfound", request.LookupId));

            return lookupDto;
        }
    }
}