using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace mpp_app_backend.Context
{
    public class AdminDataContext : IdentityDbContext
    {
        public AdminDataContext(DbContextOptions<AdminDataContext> options) : base(options)
        {
        }


    }
}
