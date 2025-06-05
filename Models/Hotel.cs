using Microsoft.IdentityModel.Tokens;

namespace Motel_birkenhein.Models
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public string Kontaktperson { get; set; }
        public string Telefon { get; set; }
        public string Private { get; set; }
        public ICollection<Zimmer> Zimmer { get; set; }
        public ICollection<Tarif> Tarif { get; set; }
        public Hotel()
        {
            Zimmer = new List<Zimmer>();
            Tarif = new List<Tarif>();
        }

    }
}
