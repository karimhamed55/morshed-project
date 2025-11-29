using System.Collections.Generic;

namespace Morshed.Core.Entities
{
    public class Tour
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        public string Title { get; set; }
        public int Days { get; set; }

        public ICollection<TourStop> Stops { get; set; }
    }
}
