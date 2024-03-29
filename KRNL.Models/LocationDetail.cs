﻿using KRNL.Data;
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
        public State State { get; set; }
        public string LocationCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GDUs { get; set; }
        public string GrowthStage { get; set; }
        public DateTimeOffset? DatePlanted { get; set; }

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
        [Display(Name ="Cooperator")]
        public string FullName { get; set; }
        public string MapLink { get; set; }
        public Crm CRM { get; set; }
        public Month MonthOfPlanting { get; set; }
        [Display(Name = "Planting Day")]
        public int DayOfPlanting { get; set; }
        [Display(Name = "Planting Year")]
        public int YearOfPlanting { get; set; }
        public string DocString { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<DocumentListItem> Documents { get; set; }
    }
}
