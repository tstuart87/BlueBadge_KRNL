using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KRNL.Data
{
    public enum Month { January = 1, February, March, April, May, June, July, August, September, October, November, December }
    public enum State { IA, IL, IN, KY, MI, MN, MO, OH, TN, WI}
    public enum Toggle { view1, view2, view3}

    //CRM = Corn Relative Maturity (days)
    public enum Crm {
        [Display(Name = "85-90")]
        CRM85_90,
        [Display(Name = "91-95")]
        CRM91_95,
        [Display(Name = "96-100")]
        CRM96_100,
        [Display(Name = "101-105")]
        CRM101_105,
        [Display(Name = "106-110")]
        CRM106_110,
        [Display(Name = "111-115")]
        CRM111_115,
        [Display(Name = "116-120")]
        CRM116_120 }

    public class Location : LocationParent
    {
        public string GrowthStage { get; set; }
        public bool IsRowbanded { get; set; }
        public Crm CRM { get; set; }
    }
}

