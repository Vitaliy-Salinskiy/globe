using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}