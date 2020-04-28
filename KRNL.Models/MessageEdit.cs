using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class MessageEdit
    {
        public int MessageId { get; set; }
        public Guid OwnerId { get; set; }
        public string Comment { get; set; }

        [Display(Name = "Cooperator")]
        public int? CooperatorId { get; set; }
        public string FullName { get; set; }

        [Display(Name = "Date")]
        public DateTimeOffset? DateCreated { get; set; }

        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Growth Stage")]
        public growthStage HumanGrowthStage { get; set; }

        [Display(Name = "Task:")]
        public job JobOne { get; set; }
        [Display(Name = "Task:")]
        public job JobTwo { get; set; }
        [Display(Name = "Task:")]
        public job JobThree { get; set; }

        public rating Rating { get; set; }
    }
}
