namespace PSA.Shared
{
    public class OrderDto
    {
        public double Suma { get; set; }
        public DateTime Data { get; set; }
        public OrderState Busena { get; set; }
        public int Id_Uzsakymas { get; set; }
        public int fk_user_id { get; set; }
    }
}