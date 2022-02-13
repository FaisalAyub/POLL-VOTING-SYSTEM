using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using PollApp.Authorization.Roles;
using PollApp.Authorization.Users;
using PollApp.MultiTenancy;

namespace PollApp.EntityFrameworkCore
{
    public class PollAppDbContext : AbpZeroDbContext<Tenant, Role, User, PollAppDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public PollAppDbContext(DbContextOptions<PollAppDbContext> options)
            : base(options)
        {
        }
    }
}
