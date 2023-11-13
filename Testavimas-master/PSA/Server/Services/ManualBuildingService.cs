using PSA.Shared;

namespace PSA.Server.Services
{
    public class ManualBuildingService : IManualBuildingService
    {
        private Robot _robot = new Robot();

        public void AddProductToBuilder(Product product)
        {
            switch (product.Category)
            {
                case 2:
                    _robot.RightLeg = product.Id;
                    break;
                case 3:
                    _robot.Head = product.Id;
                    break;
                case 4:
                    _robot.LeftArm = product.Id;
                    break;
                case 5:
                    _robot.RightArm = product.Id;
                    break;
                case 6:
                    _robot.LeftLeg = product.Id;
                    break;
                case 7:
                    _robot.Body = product.Id;
                    break;
            }
        }

        public void RemoveProductQuantityByOneFromBuilder(Product product)
        {
            //var item = _productsInCart.FirstOrDefault(x => x.Id == product.Id);
            //if (item?.Quantity == 1)
            //{
            //    _productsInCart.Remove(item);
            //}
            //else
            //{
            //    if (item != null)
            //    {
            //        item.Quantity--;
            //    }
            //}
        }

        public void RemoveProductFromBuilder(Product product)
        {
            //_productsInCart.RemoveAll(x => x.Id == product.Id);
        }

        public Robot GetBuilder()
        {
            return _robot;
        }

        public void ClearBuilder()
        {
            _robot = new Robot();
        }
    }
}
