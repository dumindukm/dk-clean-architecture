using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure.Data
{
    // inherit from Ardalis.Specification type
    public class EfRepository<T> : BaseRepository<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
    public abstract class BaseRepository<T> :  IRepositoryBase<T>, IReadRepositoryBase<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private ISpecificationEvaluator _specificationEvaluator;

        public BaseRepository(AppDbContext dbContext) : this(dbContext, SpecificationEvaluator.Default)
        {
            _dbContext = dbContext;
        }
        public BaseRepository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator)
        {
            _dbContext = dbContext;
            _specificationEvaluator = specificationEvaluator;
        }

        //public T GetById<T>(int id) where T : BaseEntity, IAggregateRoot
        //{
        //    return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        //}

        //public Task<T> GetByIdAsync<T>(int id) where T : BaseEntity, IAggregateRoot
        //{
        //    return _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        //}

        //public Task<List<T>> ListAsync<T>() where T : BaseEntity, IAggregateRoot
        //{
        //    return _dbContext.Set<T>().ToListAsync();
        //}

        //public Task<List<T>> ListAsync<T>(ISpecification<T> spec) where T : BaseEntity, IAggregateRoot
        //{
        //    var specificationResult = ApplySpecification(spec);
        //    return specificationResult.ToListAsync();
        //}

        //public async Task<T> AddAsync<T>(T entity) where T : BaseEntity, IAggregateRoot
        //{
        //    await _dbContext.Set<T>().AddAsync(entity);
        //    await _dbContext.SaveChangesAsync();

        //    return entity;
        //}

        //public Task UpdateAsync<T>(T entity) where T : BaseEntity, IAggregateRoot
        //{
        //    _dbContext.Entry(entity).State = EntityState.Modified;
        //    return _dbContext.SaveChangesAsync();
        //}

        //public Task DeleteAsync<T>(T entity) where T : BaseEntity, IAggregateRoot
        //{
        //    _dbContext.Set<T>().Remove(entity);
        //    return _dbContext.SaveChangesAsync();
        //}

        //private IQueryable<T> ApplySpecification<T>(ISpecification<T> spec) where T : BaseEntity
        //{
        //    //var evaluator = new SpecificationEvaluator<T>();
        //    //return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        //    return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        //}

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Add(entity);

            await SaveChangesAsync(cancellationToken);

            return entity;
        }
        /// <inheritdoc/>
        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;


            //await SaveChangesAsync(cancellationToken);
        }
        /// <inheritdoc/>
        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(entity);

            //await SaveChangesAsync(cancellationToken);
        }
        /// <inheritdoc/>
        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().RemoveRange(entities);

            //await SaveChangesAsync(cancellationToken);
        }
        /// <inheritdoc/>
        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }
        /// <inheritdoc/>
        public virtual async Task<T?> GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken = default) where Spec : ISpecification<T>, ISingleResultSpecification
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }
        /// <inheritdoc/>
        public virtual async Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }
        /// <inheritdoc/>
        public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }
        /// <inheritdoc/>
        public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

            return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
        }

        /// <inheritdoc/>
        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification, true).CountAsync(cancellationToken);
        }

        /// <summary>
        /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
        /// <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
        protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
        {
            return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }
        /// <summary>
        /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// <para>
        /// Projects each entity into a new form, being <typeparamref name="TResult" />.
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
        protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException("Specification is required");
            if (specification.Selector is null) throw new SelectorNotFoundException();

            return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}
