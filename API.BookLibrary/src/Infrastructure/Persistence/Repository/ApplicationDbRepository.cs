using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Domain.Contracts;
using Infrastructure.Persistence.Context;
using Mapster;
using Application.Repository;

namespace Infrastructure.Persistence.Repository
{
    public class ApplicationDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
    {
        public ApplicationDbRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
            ApplySpecification(specification, false)
                .ProjectToType<TResult>();
    }
}
