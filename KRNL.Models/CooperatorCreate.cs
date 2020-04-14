using KRNL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Models
{
    public class CooperatorCreate
    {
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name ="")]
        public string AreaCode { get; set; }
        [Display(Name = "")]
        public string PhoneFirst { get; set; }
        [Display(Name = "")]
        public string PhoneSecond { get; set; }
        [Display(Name = "Role")]
        public contact ContactType { get; set; }
    }
}
