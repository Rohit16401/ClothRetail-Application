using Domain.Services.EntityServices;
using System.Linq.Expressions;
namespace Domain.Repositories
{
    public  interface ISQLGenericRepository<T>
    {
        Task<T> GetById(string id);
        Task<IEnumerable<T>> GetByColoumn(string coloumnValue, string coloumnName);
        Task<IEnumerable<T>> GetAll();
        Task<PaginatedResult<T>> GetAll(Expression<Func<T, bool>>[] filters, int pageNumber = 1, int pageSize = 10);
        IEnumerable<T> GetAllAsync(Expression<Func<T, bool>>[] filters);
        Task<bool> Add(T entity);
        Task<T> Add(T entity, string outputParams);
        Task<bool> AddBulk(List<T> entity);
        Task<bool> Update(T entity);
        Task<bool> UpdateBulk(List<T> entity, List<string> changedColumnParam);
        Task<bool> Delete(T entity);
        Task<bool> Delete(string Id, string columnName);
        IEnumerable<T> ExecuteQuery(string query, object parameters = null);
        Task<IEnumerable<T>> GetByColumnNames(Tuple<string, string, string, string>[] filters);
        Task<bool> RecordExists(Expression<Func<T, bool>>[] filters);

        // Methods for stored procedures
        Task<bool> ExecuteStoredProcedureAsync(string? storedProcedureName = null, object parameters = null);
        Task<IEnumerable<T>> QueryStoredProcedureAsync(object parameters = null);
    }
}
