namespace Motel_birkenhein.Models
{
    public class Gast
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string StAG { get; set; }
        public string Geschlecht { get; set; }
        public string Telefon { get; set; }
        public string prvat_or_firma { get; set; }
        public string Kunden_Nr { get; set; }

        public Guid? XReservierungId { get; set; }
        public Reservierung XReservierung { get; set; }
    }
}
