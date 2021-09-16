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
        public State State { get; set; }
        [Display(Name = "Planting Date")]
        public Month MonthOfPlanting { get; set; }
        [Display(Name = " ")]
        public int DayOfPlanting { get; set; }
        [Display(Name = " ")]
        public int YearOfPlanting { get; set; }
        public string GDUs { get; set; }
        public string GrowthStage { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [Display(Name = "Planted")]
        public bool IsPlanted { get; set; }
        [Display(Name = "Staked")]
        public bool IsStaked { get; set; }
        [Display(Name = "Rowbanded")]
        public bool IsRowbanded { get; set; }
        [Display(Name = "Harvested")]
        public bool IsHarvested { get; set; }

        [Display(Name = "Cooperator")]
        public int? CooperatorId { get; set; }
        [Display(Name = "Cooperator")]
        public string FullName { get; set; }
        public string MapLink { get; set; }
        public Crm CRM { get; set; }
        [Display(Name = "#Tag")]
        public string Tag { get; set; }
        public string SearchString { get; set; }
        public string DocString { get; set; }
        public rating Rating { get; set; }
        public DateTimeOffset DatePlanted { get; set; }
        public DateTimeOffset DateHarvested { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<MessageListItem> Messages { get; set; }
        public IEnumerable<DocumentListItem> Documents { get; set; }

    }
}
