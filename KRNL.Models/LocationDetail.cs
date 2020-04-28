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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GDUs { get; set; }
        public string GrowthStage { get; set; }
        public DateTimeOffset? DatePlanted { get; set; }

        [Display(Name = "Planted")]
        public stake IsPlanted { get; set; }
        [Display(Name = "Staked")]
        public stake IsStaked { get; set; }
        [Display(Name = "Rowbanded")]
        public stake IsRowbanded { get; set; }
        [Display(Name = "Harvested")]
        public stake IsHarvested { get; set; }

        [Display(Name = "Cooperator")]
        public int? CooperatorId { get; set; }
        [Display(Name ="Cooperator")]
        public string FullName { get; set; }
        public string MapLink { get; set; }
        public crm CRM { get; set; }
        public month MonthOfPlanting { get; set; }
        [Display(Name = "Planting Day")]
        public int DayOfPlanting { get; set; }
        [Display(Name = "Planting Year")]
        public int YearOfPlanting { get; set; }
        public string DocString { get; set; }
        public IEnumerable<DocumentListItem> Documents { get; set; }
    }
}
