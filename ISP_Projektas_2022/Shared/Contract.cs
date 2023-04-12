namespace ISP_Projektas_2022.Shared
{
    public class Contract
    {
        public DateTime isdavimo_data { get; set; }
        public int id_Sutartis { get; set; }
        public int fk_Uzsakymasid_Uzsakymas { get; set; }
        public int fk_Vadybininkasid_Vadybininkas { get; set; }

        public override string? ToString()
        {
            return id_Sutartis + " | " + isdavimo_data + " | " + fk_Uzsakymasid_Uzsakymas + " | " + fk_Vadybininkasid_Vadybininkas + " | ";
        }
    }
}