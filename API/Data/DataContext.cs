using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<FileSetup> FileSetups { get; set; }
        public DbSet<AllowedFileTypes> AllowedFileTypes { get; set; }
    }
}