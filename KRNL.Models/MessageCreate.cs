using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class MessageCreate
    {
        public int MessageId { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        [Key]
        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public MessageCreate()
        {
            this.DateCreated = DateTimeOffset.Now;
        }
    }
}
