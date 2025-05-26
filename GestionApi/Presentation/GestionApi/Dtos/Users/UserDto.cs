namespace GestionApi.Dtos.Users
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string  Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
