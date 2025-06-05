namespace Motel_birkenhein.Models
{
    public class Firma
    {
        public Guid Id { get; set; }
        public string FirmeName { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string HRB { get; set; }
        public string Telefon { get; set; }
        public string Bemekung { get; set; }
        public string Kunden_Nr { get; set; }
        public Guid? XReservierungId { get; set; }
        public Reservierung XReservierung { get; set; }
    }
}
