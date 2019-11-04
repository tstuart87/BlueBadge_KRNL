using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public Guid OwnerId { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        [ForeignKey("Locations")]
        public int? LocationId { get; set; }
        public virtual Location Locations { get; set; }
    }
}
