using System.Threading.Tasks;
using PollApp.Configuration.Dto;

namespace PollApp.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
