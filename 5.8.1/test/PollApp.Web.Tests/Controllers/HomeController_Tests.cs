using System.Threading.Tasks;
using PollApp.Models.TokenAuth;
using PollApp.Web.Controllers;
using Shouldly;
using Xunit;

namespace PollApp.Web.Tests.Controllers
{
    public class HomeController_Tests: PollAppWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}