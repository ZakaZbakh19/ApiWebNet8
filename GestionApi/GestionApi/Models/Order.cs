using System.ComponentModel.DataAnnotations;

namespace GestionApi.Models
{
    public class Order : BaseModel
    {
        public long OrderNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
