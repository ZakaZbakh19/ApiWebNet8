using System.ComponentModel.DataAnnotations;

namespace GestionApi.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Order Number is required")]
        public string OrderNumber { get; set; }
    }
}
