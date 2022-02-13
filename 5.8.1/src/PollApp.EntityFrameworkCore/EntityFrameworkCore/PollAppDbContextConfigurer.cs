using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace PollApp.EntityFrameworkCore
{
    public static class PollAppDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PollAppDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PollAppDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
