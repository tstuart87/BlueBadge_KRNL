using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum Contact { Email, Text, Phone}
    public class Cooperator
    {
        [Key]
        public int CooperatorId { get; set; }
        public Guid OwnerId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Street")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        [Display(Name ="Contact Preference")]
        public Contact ContactPreference { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
    }
}
