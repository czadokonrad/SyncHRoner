using SyncHRoner.Common.Functional;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncHRoner.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<Either<Failure, bool>> DeleteAsync(long id, CancellationToken cancellationToken);
        Task<Either<Failure, Option<IEnumerable<TEntity>>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Either<Failure, Option<IEnumerable<TEntity>>>> GetAllWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<Either<Failure, Option<TEntity>>> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<Either<Failure, bool>> SaveAsync(TEntity entity, CancellationToken cancellationToken);
        Task<Either<Failure, bool>> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
