using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class MessageDetail
    {
        public int MessageId { get; set; }
        public Guid OwnerId { get; set; }
        public growthStage HumanGrowthStage { get; set; }
        public string Comment { get; set; }
        [Display(Name = "Date")]
        public DateTimeOffset? DateCreated { get; set; }
        public int? CooperatorId { get; set; }
        public string FullName { get; set; }
        public int LocationId { get; set; }
        [Display(Name ="LocID")]
        public string LocationCode { get; set; }
        public noYes IsDeleted { get; set; }
        public noYes IsRequest { get; set; }
        public job JobOne { get; set; }
        public job JobTwo { get; set; }
        public job JobThree { get; set; }
        public rating Rating { get; set; }
    }
}
