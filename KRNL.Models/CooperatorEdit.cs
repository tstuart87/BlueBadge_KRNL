using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class CooperatorEdit
    {
        public int CooperatorId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Contact Preference")]
        public Contact ContactPreference { get; set; }
        public Guid OwnerId { get; set; }
        [Display(Name = "Role")]
        public contact ContactType { get; set; }
        public noYes IsDeleted { get; set; }

    }
}
