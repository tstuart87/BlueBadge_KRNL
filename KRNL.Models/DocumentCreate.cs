﻿using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class DocumentCreate
    {
        public int DocumentId { get; set; }
        public Guid OwnerId { get; set; }
        public string DocName { get; set; }
        public string DocString { get; set; }

        [Display(Name = "Document Type")]
        public docType DocType { get; set; }
        [Display(Name = "Location")]
        public int? LocationId { get; set; }
        public bool IsDeleted { get; set; }
        public int? MessageId { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
    }
}
