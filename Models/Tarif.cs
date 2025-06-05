namespace Motel_birkenhein.Models
{
    public class Tarif
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? price { get; set; }

        public Guid? XHotelId { get; set; }
        public Hotel XHotel { get; set; }
    }
}
