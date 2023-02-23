using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsAPI.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly AppointmentDbContext _context;

    protected RepositoryBase(AppointmentDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> SearchAsync(SearchDto searchDto, CancellationToken cancellationToken)
    {
        return await _context.Set<T>()
                .Skip(searchDto.PageSize * (searchDto.PageNumber - 1))
                .Take(searchDto.PageSize)
                .AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        //var a = _context.Set<T>();
        //var b = await a.AddAsync(entity, cancellationToken);
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
