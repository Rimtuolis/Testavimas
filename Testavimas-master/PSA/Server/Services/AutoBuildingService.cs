using PSA.Services;
using PSA.Shared;

namespace PSA.Server.Services
{
    public class AutoBuildingService : IAutoBuildingService
    {
        private Robot _robot = new Robot();
        public void GenerateRobot(Robot robotG)
        {
            _robot = robotG;
        }
        public Robot GetRobot() { return  _robot; }
            
    }
}
