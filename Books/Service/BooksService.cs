using Books.Infrastructure;
using Books.Models;

namespace Books.Service
{
    public class BooksService : IBooksService
    {
        private readonly IRepository<Book> _bookRepository;

        public BooksService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> DeleteBook(int id)
        {
            return await _bookRepository.Delete(id);
        }

        public async Task<List<Book>> GetAll()
        {
            return await _bookRepository.GetAll();
        }

        public async Task<Book> GetById(int id)
        {
            return await _bookRepository.GetById(id);
        }

        public async Task<Book> SaveBook(Book book)
        {
            return await _bookRepository.Save(book);
        }

        public async Task<Book> UpdateBook(int id, Book book)
        {
            return await _bookRepository.Update(id, book);
        }
    }
}
