using System.ComponentModel.DataAnnotations;

namespace GestionApi.Dtos
{
    public class OrderDto
    {
        public Guid? Id { get; set; }
        public string OrderNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
