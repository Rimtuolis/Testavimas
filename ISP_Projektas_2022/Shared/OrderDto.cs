namespace ISP_Projektas_2022.Shared
{
    public class OrderDto
    {
        public double Suma { get; set; }
        public DateTime Data { get; set; }
        public OrderState Busena { get; set; }
        public int Id_Uzsakymas { get; set; }
        public int Fk_KlientasId_Klientas { get; set; }
        public int Fk_Sandelininkas { get; set; }
    }
}