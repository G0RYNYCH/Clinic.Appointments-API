using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;

namespace AppointmentsAPI.Interfaces;

public interface IRepositoryBase<T> where T : EntityBase
{
    Task<IEnumerable<T>> SearchAsync(SearchDto searchDto, CancellationToken cancellationToken);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task CreateAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}
