namespace DataLogic.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string HeroName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Introduction { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime? LastActive { get; set; } = null;

        public List<Photo> Photos { get; set; } = new();

    }
}
