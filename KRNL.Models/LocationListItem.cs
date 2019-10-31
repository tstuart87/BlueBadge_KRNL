﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class LocationListItem
    {
        public int LocationId { get; set; }
        [Display(Name ="Location")]
        public string LocationName { get; set; }
        [Display(Name ="LocID")]
        public string LocationCode { get; set; }
        public int? GDUs { get; set; }
        [Display(Name ="Staked")]
        public bool IsStaked { get; set; }
    }
}
