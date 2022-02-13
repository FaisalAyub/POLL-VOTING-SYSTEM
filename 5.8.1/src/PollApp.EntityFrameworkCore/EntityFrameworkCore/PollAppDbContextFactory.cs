using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PollApp.Configuration;
using PollApp.Web;

namespace PollApp.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PollAppDbContextFactory : IDesignTimeDbContextFactory<PollAppDbContext>
    {
        public PollAppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PollAppDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            PollAppDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PollAppConsts.ConnectionStringName));

            return new PollAppDbContext(builder.Options);
        }
    }
}
