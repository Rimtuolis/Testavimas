using PSA.Shared;

namespace PSA.Server.Services
{
    public interface ICurrentUserService
    {
        CurrentUser GetUser();
        void SetUser(CurrentUser user);
    }
}
