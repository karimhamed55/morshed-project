namespace Morshed.Core.Entities
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
