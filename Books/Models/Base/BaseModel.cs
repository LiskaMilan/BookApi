using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Books.Models.Base
{
    public class BaseModel
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
    }
}
