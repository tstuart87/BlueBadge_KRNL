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
        [Display(Name = "Planting Date")]
        public month MonthOfPlanting { get; set; }
        [Display(Name = " ")]
        public int DayOfPlanting { get; set; }
        [Display(Name = " ")]
        public int YearOfPlanting { get; set; }
        public string GDUs { get; set; }
        public string GrowthStage { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Display(Name = "Staked")]
        public stake IsStaked { get; set; }
        [Display(Name = "Harvested")]
        public stake IsHarvested { get; set; }
        [Display(Name = "Cooperator")]
        public int? CooperatorId { get; set; }
        [Display(Name = "Cooperator")]
        public string FullName { get; set; }
        public string MapLink { get; set; }
        public crm CRM { get; set; }
        [Display(Name = "#Tag")]
        public string Tag { get; set; }
        public string SearchString { get; set; }

        public IEnumerable<MessageListItem> Messages { get; set; }

    }
}
