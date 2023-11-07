using PSA.Shared;

namespace PSA.Server.Services
{
    public interface IManualBuildingService
    {
        void AddProductToBuilder(Product product);
        void RemoveProductQuantityByOneFromBuilder(Product product);
        Robot GetBuilder();
        void RemoveProductFromBuilder(Product product);
        void ClearBuilder();
    }
}
