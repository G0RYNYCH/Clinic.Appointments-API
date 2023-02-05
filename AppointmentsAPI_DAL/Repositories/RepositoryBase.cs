using AppointmentsAPI_DAL.Data;
using AppointmentsAPI_DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppointmentsAPI_DAL.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    private readonly AppointmentDbContext _context;

    protected RepositoryBase(AppointmentDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll() => _context.Set<T>().AsNoTracking();

    public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> condition) => _context.Set<T>().Where(condition).AsNoTracking();

    public void Create(T entity) => _context.Add(entity);

    public void Update(T entity) => _context.Update(entity);

    public void Delete(T entity) => _context.Remove(entity);
}
