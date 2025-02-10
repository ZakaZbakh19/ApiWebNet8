using System.ComponentModel.DataAnnotations;

namespace GestionApi.Models
{
    public class Order : BaseModel
    {
        [Required]
        public string OrderNumber { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
