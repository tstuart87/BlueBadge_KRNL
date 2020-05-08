using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class MessageListItem
    {
        public int MessageId { get; set; }
        public Guid OwnerId { get; set; }
        public string Comment { get; set; }
        [Display(Name ="Date")]
        public DateTimeOffset? DateCreated { get; set; }
        public int? LocationId { get; set; }
        [Display(Name = "LocID")]
        public string LocationCode { get; set; }
        [Display(Name = "Predicted Growth Stage")]
        public string PredictedGrowthStage { get; set; }
        [Display(Name ="Actual Growth Stage")]
        public growthStage HumanGrowthStage { get; set; }

        [Display(Name = "Employee")]
        public int? CooperatorId { get; set; }
        public string FullName { get; set; }
        public rating Rating { get; set; }
        public stake IsStaked { get; set; }
        public job JobOne { get; set; }
        public job JobTwo { get; set; }
        public job JobThree { get; set; }
        public noYes IsDeleted { get; set; }
        public noYes IsRequest { get; set; }
        public string SearchString { get; set; }
        public string DocString { get; set; }
        public virtual Document Documents { get; set; }

    }
}
