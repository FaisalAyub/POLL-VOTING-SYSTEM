using Abp.Authorization;
using PollApp.Authorization.Roles;
using PollApp.Authorization.Users;

namespace PollApp.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
