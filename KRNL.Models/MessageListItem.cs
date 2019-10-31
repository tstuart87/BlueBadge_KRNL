﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class MessageListItem
    {
        public int MessageId { get; set; }
        public Guid OwnerId { get; set; }
        public string Comment { get; set; }
        [Display(Name ="Date")]
        public DateTimeOffset DateCreated { get; set; }
        public int LocationId { get; set; }
    }
}