using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using PollApp.Authorization;

namespace PollApp
{
    [DependsOn(
        typeof(PollAppCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class PollAppApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<PollAppAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(PollAppApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
