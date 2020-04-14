
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRNL.Data
{
    public enum contact {
        Employee, 
        Cooperator }
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
        public string Email { get; set; }
        public string AreaCode { get; set; }
        public string PhoneFirst { get; set; }
        public string PhoneSecond { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public contact ContactType { get; set; }
    }
}
