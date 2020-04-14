using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class MessageCreate
    {
        public int MessageId { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset DateCreated { get; set; }

        [Display(Name ="Location")]
        public int LocationId { get; set; }

        [Display(Name ="LocID")]
        public string LocationCode { get; set; }
        public stake IsStaked { get; set; }

        [Display(Name = "Growth Stage")]
        public growthStage HumanGrowthStage { get; set; }
        public string PredictedGrowthStage { get; set; }

        [Display(Name = "Employee")]
        public int CooperatorId { get; set; }
        public string FullName { get; set; }

        public rating Rating { get; set; }

        [Display(Name = "Task:")]
        public job JobOne { get; set; }
        [Display(Name = "Task:")]
        public job JobTwo { get; set; }
        [Display(Name = "Task:")]
        public job JobThree { get; set; }
    }
}
