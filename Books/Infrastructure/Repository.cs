using Books.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : BaseModel, new()
    {
        private readonly LibraryContext _context;
        public Repository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Save(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(int id, T entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }

            entity.Id = existingEntity.Id;
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
