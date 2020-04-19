using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SyncHRoner.Common.Functional;
using SyncHRoner.DAL.Utils;
using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.Context;
using SyncHRoner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SyncHRoner.DAL.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        protected SyncHRonerContext Context;
        private readonly ILogger _logger;


        public BaseRepository(SyncHRonerContext context, ILogger logger)
        {
            Context = context;
            _logger = logger;
        }


        public async Task<Either<Failure, Option<TEntity>>> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                Option<TEntity> result = await Context.Set<TEntity>()
                                        .Include(Context.GetIncludePaths(typeof(TEntity)))
                                        .AsNoTracking().SingleOrDefaultAsync(GetByEntityIdExpression(id));

                return result;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }

        protected async Task<TEntity> GetByIdTrackedAsync(long id, CancellationToken cancellationToken)
        {
            return await Context.Set<TEntity>()
                .Include(Context.GetIncludePaths(typeof(TEntity)))
                .SingleOrDefaultAsync(GetByEntityIdExpression(id), cancellationToken); 
        }


        public async Task<Either<Failure, Option<IEnumerable<TEntity>>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                Option<IEnumerable<TEntity>> result = await Context.Set<TEntity>()
                                     .Include(Context.GetIncludePaths(typeof(TEntity)))
                                     .AsNoTracking()
                                     .ToListAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }

        public async Task<Either<Failure, Option<IEnumerable<TEntity>>>> GetAllIncludingAsync(params Expression<Func<TEntity, IEnumerable<object>>>[] navsToInclude)
        {
            try
            {
                IQueryable<TEntity> query = Context.Set<TEntity>().Include(Context.GetIncludePaths(typeof(TEntity)));

                //foreach (var propertyToInclude in navsToInclude)
                //{
                //    query = query.Include(propertyToInclude);
                //}

                Option<IEnumerable<TEntity>> result = await query.AsNoTracking().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }


        public async Task<Either<Failure, Option<IEnumerable<TEntity>>>> GetAllWhereAsync(Expression<Func<TEntity, bool>> predicate,
                                                                                          CancellationToken cancellationToken)
        {
            try
            {
                Option<IEnumerable<TEntity>> result = await Context.Set<TEntity>()
                                     .Include(Context.GetIncludePaths(typeof(TEntity)))
                                     .AsNoTracking()
                                     .Where(predicate)
                                     .ToListAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }


        public async Task<Either<Failure, bool>> SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                await SaveGraphAsync(entity, cancellationToken);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                if (((SqlException)ex.InnerException).Number == 2601)
                    return new Failure($"Cannot insert duplicated data");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }

        public virtual async Task<Either<Failure, bool>> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
   
                Context.Set<TEntity>().Attach(entity);

                await Context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                if (((SqlException)ex.InnerException).Number == 2601)
                    return new Failure($"Cannot insert duplicated data");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }

        public async Task<Either<Failure, bool>> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                //create empty obj with key identitfier
                TEntity t = (TEntity)Activator.CreateInstance(typeof(TEntity), true);
                t.GetType().BaseType.GetProperty("Id").GetSetMethod(true).Invoke(t, new object[] { id });

                //atach it to track changes, this way I prevent loading entity for deleting it



                Context.Set<TEntity>().Attach(t);

                Context.Set<TEntity>().Remove(t);

                await Context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in: {MethodBase.GetCurrentMethod().Name}-{GetType().Name}");
                return new Failure($"Unexpected error occured");
            }
        }

        private Expression<Func<TEntity, bool>> GetByEntityIdExpression(long id)
        {
            ParameterExpression argParam = Expression.Parameter(typeof(TEntity), "x");
            ConstantExpression entityId = Expression.Constant(id, id.GetType());
            Expression idProperty = Expression.Property(argParam, "Id");
            Expression equality = Expression.Equal(idProperty, entityId);

            return Expression.Lambda<Func<TEntity, bool>>(equality, argParam);
        }


        protected async Task SaveGraphAsync(TEntity rootEntity, CancellationToken cancellationToken)
        {
            Context.ChangeTracker.TrackGraph(
                rootEntity,
                n =>
                {

                    if (typeof(IEnumeration).IsAssignableFrom(n.Entry.Entity.GetType()))
                    {
                        n.Entry.State = EntityState.Detached;
                    }
                    else
                        n.Entry.State = EntityState.Added;

                });

            await Context.SaveChangesAsync(cancellationToken);
        }


        public void Dispose()
        {
            Context?.Dispose();
        }

    }
}
