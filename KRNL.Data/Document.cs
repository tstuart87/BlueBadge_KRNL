using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum docType {[Display(Name = "Field Book")] FieldBook, Map}
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public Guid OwnerId { get; set; }
        public string DocName { get; set; }
        public string DocString { get; set; }

        [Display(Name = "Document Type")]
        public docType DocType { get; set; }

        [ForeignKey("Locations")]
        public int? LocationId { get; set; }
        public virtual Location Locations { get; set; }
    }
}
