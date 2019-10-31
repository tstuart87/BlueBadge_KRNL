using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class LocationCreate
    {
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
    }
}
