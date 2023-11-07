namespace PSA.Shared
{
    public class ProductStats
    {
        public DateTime data { get; set; }
        public int uzsakymu_skaicius { get; set; }
        public double pelnas { get; set; }
        public int id_Statistika { get; set; }
        public int fk_Prekeid_Preke { get; set; }
    }
}