namespace GestionApi.Dtos.Orders
{
    public class OrderQuery
    {
        public Guid? Id { get; set; }
        public long? OrderNumber { get; set; }
        public bool Ascending { get; set; }
    }
}
