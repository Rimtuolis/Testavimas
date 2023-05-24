using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid double Number")]
        public double Price { get; set; }
        [Required]
        public string? Picture { get; set; }
        [Required]
        public int? Category { get; set; }
        [Required]
        public int Material { get; set; }
        [Required]
        public int? Connection { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Please enter valid integer Number")]
        public int Attack { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Please enter valid integer Number")]
        public int Defense { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Please enter valid integer Number")]
        public int Speed { get; set; }
        public int? Quantity { get; set; }

        public double? Total { get { return Quantity * Price; } }

    }
}