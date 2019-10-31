using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum Dimension { Length, Width}
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        [Display(Name ="Location")]
        public string LocationName { get; set; }
        [Required]
        [Display(Name ="LocID")]
        public string LocationCode { get; set; }
        public Guid OwnerId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int? NumberOfPlots { get; set; }
        public Dimension? KnownDimension { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public int? GDUs { get; set; }
        public DateTimeOffset? DatePlanted { get; set; }
        public bool IsStaked { get; set; }
        public int? Year { get; set; }
        
        [ForeignKey("Cooperators")]
        public int? CooperatorId { get; set; }
        public virtual Cooperator Cooperators { get; set; }
    }
}
