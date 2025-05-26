using System.ComponentModel.DataAnnotations;

namespace GestionApi.Dtos.Orders
{
    public class OrderDto : OrderBaseDto
    {
        public Guid? Id { get; set; }
        public long OrderNumber { get; set; }
    }
}
