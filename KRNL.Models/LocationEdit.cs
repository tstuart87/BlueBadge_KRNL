using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public enum Dimension { Length, Width }

    public class LocationEdit
    {
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "Location")]
        public string LocationName { get; set; }
        [Required]
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        [Display(Name ="# of Plots")]
        public int? NumberOfPlots { get; set; }
        public Dimension? KnownDimension { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        [Display(Name ="Planting Date")]
        public DateTimeOffset? DatePlanted { get; set; }
        [Display(Name ="Staked")]
        public bool IsStaked { get; set; }
        public int? Year { get; set; }
        [Display(Name ="Cooperator")]
        public int CooperatorId { get; set; }
    }
}
