namespace GestionApi.Dtos.Users
{
    public class RequestAuthDto
    {
        public required string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
