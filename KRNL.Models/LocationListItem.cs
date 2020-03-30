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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GDUs { get; set; }
        [Display(Name = "Growth Stage")]
        public string GrowthStage { get; set; }
        [Display(Name = "Staked")]
        public stake IsStaked { get; set; }
        public string MapLink { get; set; }
    }
}
