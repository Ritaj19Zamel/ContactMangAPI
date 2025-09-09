
namespace ContactMangAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required, MaxLength(200)]
        public string Password { get; set; } = null!;
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}

