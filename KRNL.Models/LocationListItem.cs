using KRNL.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KRNL.Models
{
    public class LocationListItem
    {
        public int LocationId { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        public state State { get; set; }
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string GDUs { get; set; }
        [Display(Name = "Staked")]
        public bool IsStaked { get; set; }
        public string MapLink { get; set; }
    }
}
