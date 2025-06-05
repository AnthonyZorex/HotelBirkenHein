namespace Motel_birkenhein.Models
{
    public class Zimmer
    {
        public  Guid Id { get; set; }
        public string Zimmernummer { get; set; }
        
        public Guid? XReservierungId { get; set; }
        public Reservierung XReservierung { get; set; }

        public Guid? XHotelId { get; set; }
        public Hotel XHotels { get; set; }
        
        public ICollection<Bett> Bett { get; set; }
        public Zimmer()
        {
            Bett = new List<Bett>();
        }
    }
}
