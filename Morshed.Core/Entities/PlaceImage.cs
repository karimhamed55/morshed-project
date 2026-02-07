namespace Morshed.Core.Entities
{
    public class PlaceImage
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public string Url { get; set; }
    }
}
