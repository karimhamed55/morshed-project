using System.Collections.Generic;

namespace Morshed.Core.Entities
{
    public class Province
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }

        public ICollection<Place> Places { get; set; }
    }
}
