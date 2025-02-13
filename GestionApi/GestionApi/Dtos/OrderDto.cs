using System.ComponentModel.DataAnnotations;

namespace GestionApi.Dtos
{
    public class OrderDto : OrderBaseDto
    {
        public Guid? Id { get; set; }
        public long OrderNumber { get; set; }
    }
}
