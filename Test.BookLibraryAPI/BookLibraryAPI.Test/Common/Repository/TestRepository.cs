using Application.Repository;
using Ardalis.Specification;
using Domain.Common.Contracts;
using Mapster;
namespace Test.BookLibraryAPI.Common.Repository
{
    public class TestRepository<T> : IRepository<T>, IReadRepository<T>
    where T : AuditableEntity<Guid>, IAggregateRoot
    {
        public readonly List<T> Entities = new List<T>();
        public TestRepository()
        {
        }

        public TestRepository(List<T> data)
        {
            Entities = data;
        }

        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            Entities.Add(entity);
            return Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            Entities.AddRange(entities);
            return await Task.FromResult(entities);
        }

        public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Entities.AsQueryable().Any(specification.WhereExpressions?.FirstOrDefault()?.Filter!));
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Entities.Any());
        }

        public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Entities.AsQueryable().Count(specification.WhereExpressions?.FirstOrDefault()?.Filter!));
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Entities.Count());
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            Entities.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                Entities.Remove(entity);
            }

            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await Entities.ToAsyncEnumerable().FirstOrDefaultAsync(specification.WhereExpressions?.FirstOrDefault()?.Filter.Compile()!, cancellationToken);
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var result = await Entities.ToAsyncEnumerable().FirstOrDefaultAsync(specification.WhereExpressions?.FirstOrDefault()?.Filter.Compile()!, cancellationToken);
            return result is not null ? result.Adapt<TResult>() : default;

        }

        public Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
            where TId : notnull
        {
            return Task.FromResult(Entities.Find(e => e.Id.Equals(id)));
        }

        public Task<T?> GetBySpecAsync<TSpec>(TSpec specification, CancellationToken cancellationToken = default)
            where TSpec : ISingleResultSpecification, ISpecification<T>
        {
            return Task.FromResult(Entities.AsQueryable().Where(specification.WhereExpressions?.FirstOrDefault()?.Filter!).FirstOrDefault());
        }

        public async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var result = await Task.Run(() => Entities.AsQueryable().Where(specification.WhereExpressions?.FirstOrDefault()?.Filter!).FirstOrDefault());
            return result is not null ? result.Adapt<TResult>() : default;
        }

        public async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await FirstOrDefaultAsync(specification, cancellationToken);
        }

        public Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Entities);
        }

        public Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            if (specification.WhereExpressions.Count() == 0) return Task.FromResult(new List<T>());

            return Task.FromResult(Entities.AsQueryable().Where(specification.WhereExpressions?.FirstOrDefault()?.Filter!).ToList());
        }

        public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var where = specification.WhereExpressions.FirstOrDefault();
            var orderBy = specification.OrderExpressions.FirstOrDefault();

            if (where is not null && orderBy is null)
            {
                return Task.FromResult(Entities.AsQueryable().Where(where.Filter).Adapt<List<TResult>>());
            }
            else if (where is null && orderBy is not null)
            {
                return Task.FromResult(Entities.AsQueryable().OrderBy(orderBy.KeySelector).Adapt<List<TResult>>());
            }
            else if (where is not null && orderBy is not null)
            {
                return Task.FromResult(Entities.AsQueryable().Where(where.Filter).OrderBy(orderBy.KeySelector).Adapt<List<TResult>>());
            }

            return Task.FromResult(Entities.AsQueryable().Adapt<List<TResult>>());
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(1);
        }

        public T? SingleOrDefault(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Entities.AsQueryable().Where(specification.WhereExpressions?.SingleOrDefault()?.Filter!).Adapt<T>();
        }

        public async Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Entities.AsQueryable().Where(specification.WhereExpressions.SingleOrDefault().Filter).Adapt<T>();
        }

        public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            int id = Entities.FindIndex(e => e.Id == entity.Id);
            Entities[id] = entity;

            return Task.CompletedTask;
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken);
            }
        }
    }
}
