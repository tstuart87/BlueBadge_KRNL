using KRNL.Data;
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
        [Display(Name = "City")]
        public string LocationName { get; set; }
        public State State { get; set; }
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
        public Crm CRM { get; set; }
        public string SearchString { get; set; }
        public bool IsDeleted { get; set; }
    }
}
