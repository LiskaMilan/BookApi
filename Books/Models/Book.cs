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
        [Range(1000, 9999, ErrorMessage = "Year must be between 1000 and 9999")]
        public int Year { get; set; }
    }
}
