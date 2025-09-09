
namespace ContactMangAPI.DTOs
{
    public class ContactDto
    {
        [Required] 
        public string FirstName { get; set; } = null!;
        [Required] 
        public string LastName { get; set; } = null!;
        [Required, Phone] 
        public string PhoneNumber { get; set; } = null!;
        [Required, EmailAddress] 
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}
