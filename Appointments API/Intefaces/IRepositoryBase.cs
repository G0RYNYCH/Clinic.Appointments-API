using Appointments_API.Models;
using Appointments_API.Models.Dto;

namespace Appointments_API.Intefaces;

public interface IRepositoryBase<T> where T : EntityBase
{
    Task<IEnumerable<T>> Search(SearchDto searchDto);

    Task<IEnumerable<T>> GetAll();

    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task CreateAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}
