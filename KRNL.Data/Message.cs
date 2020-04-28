using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum growthStage {
        [Display(Name = " ")]
        blank, 
        V0, VE, V1, V2, V3, V4, V5, V6, V7, V8, V9, V10, V11, V12, V13, V14, V15, VT, R1, R2, R3, R4, R5, R6 }

    public enum rating {
        [Display(Name = " ")]
        NoRating,
        Excellent = 5, 
        [Display(Name = "Above Average")]
        AboveAverage = 4, 
        Average = 3,
        [Display(Name = "Below Average")]
        BelowAverage = 2, 
        Poor = 1};

    public enum job
    {
        [Display(Name = " ")]
        blank,
        Cultivating,
        [Display(Name = "Drone Flight")]
        DroneFlight,
        Harvesting,
        [Display(Name = "Herbicide Application")]
        HerbicideSpraying,
        [Display(Name = "Insecticide Application")]
        InsecticideSpraying,
        [Display(Name = "Nitrogen Application")]
        NitrogenApplication,
        [Display(Name = "Note Taking")]
        Notes,
        [Display(Name = "Plant & Ear Heights")]
        PlantEarHeights,
        Planting,
        Rowbanding,
        Staking,
        [Display(Name = "Stand Counts")]
        StandCounts,
        [Display(Name = "Weed Management")]
        WeedManagement
    }

    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public Guid OwnerId { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset? DateCreated { get; set; }

        [Display(Name = "LocID")]
        public string LocationCode { get; set; }

        public job JobOne { get; set; }
        public job JobTwo { get; set; }
        public job JobThree { get; set; }

        public growthStage HumanGrowthStage { get; set; }
        public string PredictedGrowthStage { get; set; }

        public rating Rating { get; set; }

        [ForeignKey("Cooperators")]
        public int? CooperatorId { get; set; }
        public virtual Cooperator Cooperators { get; set; }
        public string FullName { get; set; }

        [ForeignKey("Locations")]
        [Required]
        public int LocationId { get; set; }
        public virtual Location Locations { get; set; }
    }
}
