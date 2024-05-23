using Books.Controllers;
using Books.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Book.UnitTests
{
    public class BookApiUnitTests
    {
        public BookApiUnitTests()
        {
        }
        
       [Fact]
        public async Task GetAllBooks_ReturnsListOfBooks()
        {
            // Arrange
            var mockService = new Mock<IBooksService>();
            var expectedBooks = new List<Books.Models.Book>
            {
                new Books.Models.Book { Id = 1, Title = "Book 1", Year = 1994 },
                new Books.Models.Book { Id = 2, Title = "Book 2", Year = 1894 },
                new Books.Models.Book { Id = 3, Title = "Book 3", Year = 1694 }
            };
            mockService.Setup(service => service.GetAll()).ReturnsAsync(expectedBooks);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Books.Models.Book>>>(result);
            var books = Assert.IsAssignableFrom<IEnumerable<Books.Models.Book>>(okResult.Value);
            Assert.Equal(expectedBooks.Count, books.Count());
        }

        [Fact]
        public async Task GetBook_WithValidId_ReturnsBook()
        {
            // Arrange
            int id = 1;
            var expectedBook = new Books.Models.Book { Id = id, Title = "Test Book", Year = 2005 };
            var mockService = new Mock<IBooksService>();
            mockService.Setup(service => service.GetById(id)).ReturnsAsync(expectedBook);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.GetBook(id);

            // Assert
            var okResult = Assert.IsType<ActionResult<Books.Models.Book>>(result);
            var book = Assert.IsType<Books.Models.Book>(okResult.Value);
            Assert.Equal(expectedBook.Id, book.Id);
            Assert.Equal(expectedBook.Title, book.Title);
        }

        [Fact]
        public async Task GetBook_WithInValidId_ReturnsNotFound()
        {
            // Arrange
            int id = 1;
            var mockService = new Mock<IBooksService>();
            mockService.Setup(service => service.GetById(id)).ReturnsAsync((Books.Models.Book)null);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.GetBook(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result); 
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ValidModelState_ReturnsCreatedAtAction()
        {
            // Arrange
            var validBook = new Books.Models.Book { Title = "Test Book", Year = 2000 };
            var mockService = new Mock<IBooksService>();
            mockService.Setup(service => service.SaveBook(validBook)).ReturnsAsync(validBook);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.CreateBook(validBook);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var book = Assert.IsType<Books.Models.Book>(createdAtActionResult.Value);
            Assert.Equal(validBook, book); 
        }

        [Fact]
        public async Task CreateBook_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var invalidBook = new Books.Models.Book {Author = "Author", Year = 1999 }; 
            var mockService = new Mock<IBooksService>();
            var controller = new BooksController(mockService.Object);
            controller.ModelState.AddModelError("Title", "The Title field is required."); 

            // Act
            var result = await controller.CreateBook(invalidBook);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result); 
        }

        [Fact]
        public async Task UpdateBook_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var invalidBook = new Books.Models.Book();
            var mockService = new Mock<IBooksService>();
            var controller = new BooksController(mockService.Object);
            controller.ModelState.AddModelError("Title", "The Title field is required."); 

            // Act
            var result = await controller.UpdateBook(id, invalidBook);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBook_UpdatedBookIsNull_ReturnsNotFound()
        {
            // Arrange
            int id = 1;
            var book = new Books.Models.Book { Id = id, Title = "Test Book" };
            var mockService = new Mock<IBooksService>();
            mockService.Setup(service => service.UpdateBook(id, book)).ReturnsAsync((Books.Models.Book)null);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.UpdateBook(id, book);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Kniha s id: {id} sa nenašla", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteBook_BookNotFound_ReturnsNotFound()
        {
            // Arrange
            int id = 1;
            var mockService = new Mock<IBooksService>();
            mockService.Setup(service => service.DeleteBook(id)).ReturnsAsync((Books.Models.Book)null);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.DeleteBook(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteBook_BookDeleted_ReturnsOkWithMessage()
        {
            // Arrange
            int id = 1;
            var book = new Books.Models.Book { Id = id, Title = "Test Book", Year = 2000 };
            var mockService = new Mock<IBooksService>();
            mockService.Setup(service => service.DeleteBook(id)).ReturnsAsync(book);
            var controller = new BooksController(mockService.Object);

            // Act
            var result = await controller.DeleteBook(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}