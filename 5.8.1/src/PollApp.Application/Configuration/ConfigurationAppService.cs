using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using PollApp.Configuration.Dto;

namespace PollApp.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : PollAppAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
