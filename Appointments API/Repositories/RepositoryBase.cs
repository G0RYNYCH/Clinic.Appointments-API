using Appointments_API.Intefaces;
using Appointments_API.Models;
using Appointments_API.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Appointments_API.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly AppointmentDbContext _context;

    protected RepositoryBase(AppointmentDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> Search(SearchDto searchDto)
    {
        return await _context.Set<T>()
                .Skip(searchDto.PageSize * (searchDto.PageNumber - 1))
                .Take(searchDto.PageSize)
                .AsNoTracking().ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken) //TODO: return type
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
