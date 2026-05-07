using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Ease.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}