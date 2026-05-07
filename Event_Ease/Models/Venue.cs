using System.ComponentModel.DataAnnotations;

namespace Event_Ease.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }
    }
}