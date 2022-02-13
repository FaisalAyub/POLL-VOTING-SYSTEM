using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PollApp.EntityFrameworkCore;
using PollApp.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace PollApp.Web.Tests
{
    [DependsOn(
        typeof(PollAppWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class PollAppWebTestModule : AbpModule
    {
        public PollAppWebTestModule(PollAppEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PollAppWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(PollAppWebMvcModule).Assembly);
        }
    }
}