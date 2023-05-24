using PSA.Shared;

namespace PSA.Server.Services
{
    public interface IAutoBuildingService
    {
        void GenerateRobot(Robot robotG);
        Robot GetRobot();
    }
}
