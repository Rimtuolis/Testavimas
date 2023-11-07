namespace PSA.Shared
{
    public class OrderCreateDto
    {
        public List<int> ProductIds { get; set; }
        public double Total { get; set; }

        public OrderCreateDto(List<int> productIds, double total)
        {
            ProductIds = productIds;
            Total = total;
        }

    }
}
