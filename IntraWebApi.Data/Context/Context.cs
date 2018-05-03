using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace IntraWebApi.Data.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            :base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccessRight> UserAccessRights { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }
        public DbSet<Article> Articles { get; set; }
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

    public class Article
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        public byte[] Picture { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
