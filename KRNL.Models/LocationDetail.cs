using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class LocationDetail
    {
        public int LocationId { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Display(Name = "LocID")]
        public state State { get; set; }
        public string LocationCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string GDUs { get; set; }
        public DateTimeOffset? DatePlanted { get; set; }
        [Display(Name = "Staked")]
        public bool IsStaked { get; set; }
        public int? CooperatorId { get; set; }
        [Display(Name ="Cooperator")]
        public string FullName { get; set; }
        public string MapLink { get; set; }
    }
}
