using System.Linq.Expressions;

namespace Appointments_API.Data.Intefaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(Guid entity, CancellationToken cancellationToken);
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}
