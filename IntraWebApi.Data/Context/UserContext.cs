using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IntraWebApi.Data.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccessRight> UserAccessRights { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Civility { get; set; }

    }

    public class UserCredentials
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
    }

    public class UserAccessRight
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
    }
}
