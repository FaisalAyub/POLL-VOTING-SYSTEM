using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace PollApp.Controllers
{
    public abstract class PollAppControllerBase: AbpController
    {
        protected PollAppControllerBase()
        {
            LocalizationSourceName = PollAppConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
