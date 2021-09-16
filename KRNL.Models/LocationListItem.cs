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
        public State State { get; set; }
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GDUs { get; set; }
        [Display(Name = "Growth Stage")]
        public string GrowthStage { get; set; }

        [Display(Name = "Planted")]
        public bool IsPlanted { get; set; }
        [Display(Name = "Staked")]
        public bool IsStaked { get; set; }
        [Display(Name = "Rowbanded")]
        public bool IsRowbanded { get; set; }
        [Display(Name = "Harvested")]
        public bool IsHarvested { get; set; }

        public string MapLink { get; set; }
        public Month MonthOfPlanting { get; set; }
        public int DayOfPlanting { get; set; }
        public int YearOfPlanting { get; set; }
        public string SearchString { get; set; }
        public string DocString { get; set; }
        public bool IsDeleted { get; set; }
        public int RequestCount { get; set; }
        public rating Rating { get; set; }
        public string LastVisitor { get; set; }

        public virtual Message Messages { get; set; }
        public virtual Document Documents { get; set; }

        public int? CooperatorId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
