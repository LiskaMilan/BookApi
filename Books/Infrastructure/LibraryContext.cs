using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
    }
}
