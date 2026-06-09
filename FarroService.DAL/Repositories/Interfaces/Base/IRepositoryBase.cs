using System.Linq.Expressions;

namespace FarroService.DAL.Repositories.Interfaces.Base;

/// <summary>
/// A generic base interface for repository implementations.
/// </summary>
/// <typeparam name="T">The type of the domain entity.</typeparam>
public interface IRepositoryBase<T>
{
    /// <summary>
    /// Retrieves all records of type T from the database without tracking changes.
    /// </summary>
    IQueryable<T> FindAll();

    /// <summary>
    /// Retrieves records matching the specified condition without tracking changes.
    /// </summary>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Prepares a new entity of type T to be inserted into the database.
    /// </summary>
    void Create(T entity);

    /// <summary>
    /// Prepares an existing entity of type T to be updated.
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Prepares an existing entity of type T to be deleted.
    /// </summary>
    void Delete(T entity);
}
