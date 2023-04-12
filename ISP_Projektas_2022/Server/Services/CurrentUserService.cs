using ISP_Projektas_2022.Shared;

namespace ISP_Projektas_2022.Server.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private CurrentUser? _user;

        public CurrentUser GetUser()
        {
            return _user ?? new CurrentUser { LoggedIn = false };
        }

        public void SetUser(CurrentUser user)
        {
            _user = user;
        }
    }
}
