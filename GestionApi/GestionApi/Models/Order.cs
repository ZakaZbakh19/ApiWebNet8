using System.ComponentModel.DataAnnotations;

namespace GestionApi.Models
{
    public class Order : BaseModel
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
