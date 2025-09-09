
namespace ContactMangAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
