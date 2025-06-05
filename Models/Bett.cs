using Microsoft.Identity.Client;

namespace Motel_birkenhein.Models
{
    public class Bett
    {
        public Guid Id { get; set; }
        public string BettName { get; set; }
        public string? Gast { get; set; }

        public Guid XZimmerId { get; set; }
        public Zimmer XZimmer { get; set; }

        public Guid? XReservierungId { get; set; }
        public Reservierung XReservierung { get; set; }
    }
}
