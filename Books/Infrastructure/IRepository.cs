using Books.Models.Base;

namespace Books.Infrastructure
{
    public interface IRepository<T> where T : BaseModel, new()
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Save(T entity);
        Task<T?> Update(int id, T entity);
        Task<T?> Delete(int id);
    }
}
