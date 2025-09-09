namespace ContactMangAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();
            modelBuilder.Entity<Contact>()
                       .HasIndex(c => new { c.UserId, c.Email })
                       .IsUnique();
            modelBuilder.Entity<Contact>().HasData(
                   new Contact { Id = 1, UserId = 1, FirstName = "Sara", LastName = "Hassan", Email = "sara.hassan@example.com", PhoneNumber = "+201001234567", BirthDate = new DateTime(2000, 12, 5) },
                   new Contact { Id = 2, UserId = 1, FirstName = "Ali", LastName = "Khaled", Email = "ali.khaled@example.com", PhoneNumber = "+201112223334", BirthDate = new DateTime(1998, 3, 22) },
                   new Contact { Id = 3, UserId = 1, FirstName = "Mona", LastName = "Adel", Email = "mona.adel@example.com", PhoneNumber = "+201223344556", BirthDate = new DateTime(1995, 7, 15) },
                   new Contact { Id = 4, UserId = 1, FirstName = "Hany", LastName = "Tarek", Email = "hany.tarek@example.com", PhoneNumber = "+201334455667", BirthDate = new DateTime(1992, 11, 3) },
                   new Contact { Id = 5, UserId = 1, FirstName = "Nour", LastName = "Salem", Email = "nour.salem@example.com", PhoneNumber = "+201445566778", BirthDate = new DateTime(1999, 5, 20) },
                   new Contact { Id = 6, UserId = 1, FirstName = "Youssef", LastName = "Ibrahim", Email = "youssef.ibrahim@example.com", PhoneNumber = "+201556677889", BirthDate = new DateTime(2001, 8, 30) },
                   new Contact { Id = 7, UserId = 1, FirstName = "Laila", LastName = "Mahmoud", Email = "laila.mahmoud@example.com", PhoneNumber = "+201667788990", BirthDate = new DateTime(1996, 9, 12) },
                   new Contact { Id = 8, UserId = 1, FirstName = "Karim", LastName = "Omar", Email = "karim.omar@example.com", PhoneNumber = "+201778899001", BirthDate = new DateTime(1994, 2, 18) },
                   new Contact { Id = 9, UserId = 1, FirstName = "Dina", LastName = "Fouad", Email = "dina.fouad@example.com", PhoneNumber = "+201889900112", BirthDate = new DateTime(1997, 6, 27) },
                   new Contact { Id = 10, UserId = 1, FirstName = "Mostafa", LastName = "Samir", Email = "mostafa.samir@example.com", PhoneNumber = "+201990011223", BirthDate = new DateTime(1993, 10, 8) }
               );
            base.OnModelCreating(modelBuilder);
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
