using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRNL.Data;

namespace KRNL.Models
{
    public class LocationEdit
    {
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Required]
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
        public state State { get; set; }
        public month MonthOfPlanting { get; set; }
        public int DayOfPlanting { get; set; }
        public int YearOfPlanting { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public bool IsStaked { get; set; }
        public int CooperatorId { get; set; }
        [Display(Name = "Cooperator")]
        public string FullName { get; set; }

    }
}
