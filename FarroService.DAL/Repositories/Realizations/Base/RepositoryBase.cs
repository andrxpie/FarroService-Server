using FarroService.DAL.Persistence;
using FarroService.DAL.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FarroService.DAL.Repositories.Realizations.Base;

/// <summary>
/// A generic abstract class implementing base repository functions.
/// </summary>
/// <typeparam name="T">The type of the domain entity.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected FarroServiceDbContext RepositoryContext { get; set; }

    protected RepositoryBase(FarroServiceDbContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> FindAll() =>
        RepositoryContext.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
        RepositoryContext.Set<T>().Where(expression).AsNoTracking();

    public void Create(T entity) =>
        RepositoryContext.Set<T>().Add(entity);

    public void Update(T entity) =>
        RepositoryContext.Set<T>().Update(entity);

    public void Delete(T entity) =>
        RepositoryContext.Set<T>().Remove(entity);
}
