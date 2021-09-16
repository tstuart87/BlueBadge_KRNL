using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public class LocationParent
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }

        public Guid OwnerId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string GDUs { get; set; }
        public string CumulativePrecip { get; set; }
        public Month MonthOfPlanting { get; set; }
        public int DayOfPlanting { get; set; }
        public int YearOfPlanting { get; set; }

        public Month MonthOfHarvest { get; set; }
        public int DayOfHarvest { get; set; }
        public int YearOfHarvest { get; set; }

        public DateTimeOffset DatePlanted { get; set; }
        public DateTimeOffset DateHarvested { get; set; }

        public bool IsPlanted { get; set; }
        public bool IsStaked { get; set; }
        public bool IsHarvested { get; set; }

        public string MapLink { get; set; }

        [Display(Name = "#")]
        public string Tag { get; set; }
        public string SearchString { get; set; }

        public rating Rating { get; set; }
        public int RequestCount { get; set; }
        public string LastVisitor { get; set; }

        public bool IsDeleted { get; set; }

        public virtual IEnumerable<Message> Messages { get; set; }
        public virtual IEnumerable<Document> Documents { get; set; }

        [ForeignKey("Cooperators")]
        public int? CooperatorId { get; set; }
        public virtual Cooperator Cooperators { get; set; }
        public string Map { get; set; }
    }
}
