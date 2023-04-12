using ISP_Projektas_2022.Shared;

namespace ISP_Projektas_2022.Server.Services
{
    public interface ICurrentUserService
    {
        CurrentUser GetUser();
        void SetUser(CurrentUser user);
    }
}
