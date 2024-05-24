using Books.Models;

namespace Books.Service
{
    public interface IBooksService
    {
        Task<List<Book>> GetAll();
        Task<Book?> GetById(int id);
        Task<Book> SaveBook(Book book);
        Task<Book?> UpdateBook(int id, Book book);
        Task<Book> DeleteBook(int id);

    }
}
