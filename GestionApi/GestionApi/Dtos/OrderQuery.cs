namespace GestionApi.Dtos
{
    public class OrderQuery
    {
        public Guid? Id { get; set; }
        public long? OrderNumber { get; set; }
        public bool Ascending { get; set; }
    }
}
