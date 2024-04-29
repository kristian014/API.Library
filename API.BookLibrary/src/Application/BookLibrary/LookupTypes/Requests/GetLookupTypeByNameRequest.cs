using Application.BookLibrary.LookupTypes.Dto;
using Application.BookLibrary.LookupTypes.Spec;
using Application.Common.Exceptions;
using Application.Repository;
using Domain.Models;
using Mapster;

namespace Application.BookLibrary.LookupTypes.Requests
{
    public class GetLookupTypeByNameRequest : IRequest<LookupTypeDto>
    {
        public string Name { get; set; }

        public GetLookupTypeByNameRequest(string name) => Name = name;
    }

    public class GetLookupTypeByNameHandler : IRequestHandler<GetLookupTypeByNameRequest, LookupTypeDto>
    {
        private readonly IReadRepository<LookupType> _repository;

        public GetLookupTypeByNameHandler(IReadRepository<LookupType> repository) =>
            (_repository) = (repository);

        public async Task<LookupTypeDto> Handle(GetLookupTypeByNameRequest request, CancellationToken cancellationToken)
        {
            LookupTypeDto? lookupType = await _repository.FirstOrDefaultAsync(new LookTypeByNameSpec(request.Name), cancellationToken);

            _ = lookupType ?? throw new NotFoundException("lookuptype.notfound");

            return lookupType.Adapt<LookupTypeDto>();
        }
    }
}