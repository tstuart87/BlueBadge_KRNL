using KRNL.WebMVC.Data;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum month { January = 1, February, March, April, May, June, July, August, September, October, November, December }
    public enum state { IA, IL, IN, KY, MI, MN, MO, OH, TN, WI}
    public enum stake { No, Yes}
    public enum crm {
        [Display(Name = "85-90")]
        CRM85_90,
        [Display(Name = "91-95")]
        CRM91_95,
        [Display(Name = "96-100")]
        CRM96_100,
        [Display(Name = "101-105")]
        CRM101_105,
        [Display(Name = "106-110")]
        CRM106_110,
        [Display(Name = "111-115")]
        CRM111_115,
        [Display(Name = "116-120")]
        CRM116_120 }

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
        public stake IsStaked { get; set; }
        public string MapLink { get; set; }
        public crm CRM { get; set; }

        [Display(Name ="#")]
        public string Tag { get; set; }
        public string SearchString { get; set; }
        public stake IsHarvested { get; set; }
        public rating Rating { get; set; }

        public virtual IEnumerable<Message> Messages { get; set; }

        [ForeignKey("Cooperators")]
        public int? CooperatorId { get; set; }
        public virtual Cooperator Cooperators { get; set; }
    }
}

