using Books.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class Book : BaseModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Author { get; set; }
        [Required(ErrorMessage = "Year is required")]
        public int Year { get; set; }
    }
}
