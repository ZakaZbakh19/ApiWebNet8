namespace GestionApi.Dtos
{
    public abstract class OrderBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
