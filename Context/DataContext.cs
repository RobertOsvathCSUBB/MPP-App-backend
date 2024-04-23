using Microsoft.EntityFrameworkCore;
using mpp_app_backend.Models;

namespace mpp_app_backend.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<LoginActivity> LoginActivities { get; set; }
    }
}
