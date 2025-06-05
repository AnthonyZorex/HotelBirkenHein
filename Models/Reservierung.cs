namespace Motel_birkenhein.Models
{
    public class Reservierung
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Zimmernummer { get; set; }
        public float? Zahlen { get; set; }
        public float? Muss_bezahlen { get; set; }
        public DateTime? Reserviertvod { get; set; }
        public DateTime? Reserviertbis { get; set; }
        public string Zahlungsart { get; set; }
        public string? Rechnungsempf { get; set; }
        public string? Kontakt { get; set; } 
        public string? NameGast { get; set; }
        public string? GastKontakt { get; set; }
        public string? Bemerkung { get; set; }
        public bool? Rcontinue { get; set; }

    public string? AllBett { get; set; }

        public ICollection<Gast> Gast { get; set; }
        public ICollection<Firma> Firma { get; set; }
        public ICollection<Zimmer> Zimmer { get; set; }
        public ICollection<Bett> Bett { get; set; }
        public Reservierung()
        {
            Gast = new List<Gast>();
            Firma = new List<Firma>();
            Zimmer = new List<Zimmer>();
            Bett = new List<Bett>();
        }
    }
}
