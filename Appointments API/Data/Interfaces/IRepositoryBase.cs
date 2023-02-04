using System.Linq.Expressions;

namespace Appointments_API.Data.Interfaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
