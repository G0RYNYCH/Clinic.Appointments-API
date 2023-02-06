using Appointments_API.Data.Intefaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appointments_API.Data.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly AppointmentDbContext _context;

    protected RepositoryBase(AppointmentDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll() => _context.Set<T>().AsNoTracking();

    //public async Task<List<T>> FindAllByConditionAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    //{
    //    return await _context.Set<T>()
    //                         .Where(condition)
    //                         .AsNoTracking()
    //                         .ToListAsync<T>(cancellationToken); //TODO:
    //}

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.AddAsync(entity, cancellationToken); //TODO: await _context.Set<T>().AddAsync(entity, cancellationToken);
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
