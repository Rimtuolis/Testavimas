namespace PSA.Shared
{
    public class Product
    {
        //public string? Pavadinimas { get; set; }
        //public DateTime Pagaminimo_Data { get; set; }
        //public double Kaina { get; set; }
        //public string? Miestas { get; set; }
        //public string? Modelis { get; set; }
        //public string? Aprasymas { get; set; }
        //public int? Kiekis { get; set; }
        //public Manufacturer Gamintojas { get; set; }
        //public Category Kategorija { get; set; }
        //public Quality Kokybe { get; set; }
        //public string? Nuotrauka { get; set; }
        //public int Id_Preke { get; set; }
        //public int Fk_Tiekejasid_Tiekejas { get; set; }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Picture { get; set; }

        public int? Quantity { get; set; }

        public double? Total { get { return Quantity * Price; } }

    }
}