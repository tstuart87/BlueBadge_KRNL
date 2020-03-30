using KRNL.WebMVC.Data;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum month { January = 1, February, March, April, May, June, July, August, September, October, November, December }
    public enum state { IA, IL, IN, KY, MI, MN, MO, OH, TN, WI}
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Required]
        public state State { get; set; }

        [Required]
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }

        public Guid OwnerId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string GDUs { get; set; }
        public string CumulativePrecip { get; set; }
        public string GrowthStage { get; set; }
        public month MonthOfPlanting { get; set; }
        public int DayOfPlanting { get; set; }
        public int YearOfPlanting { get; set; }
        public DateTimeOffset DatePlanted { get; set; }
        public bool IsStaked { get; set; }
        public string MapLink { get; set; }

        [ForeignKey("Cooperators")]
        public int? CooperatorId { get; set; }
        public virtual Cooperator Cooperators { get; set; }
    }
}
