using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public enum Contact { Email, Text, Phone }
    public class CooperatorListItem
    {
        [Display(Name = "Cooperator")]
        public int CooperatorId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid OwnerId { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Display(Name = "Role")]
        public contact ContactType { get; set; }
        public noYes IsDeleted { get; set; }

    }
}
