using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fuhua.Models
{
    public class AmountModels
    {
        [Display(Name = "标的名称")]
        public string pjName { get; set; }

        [Display(Name = "标的金额")]
        public decimal? Moneys { get; set; }

        [Display(Name ="年化收益率（%）")]
        public decimal? Yearget { get; set; }

        [Display(Name ="投资期限（月）")]
        public int? TimeLimit { get; set; }

        [Display(Name ="投资金额")]
        public decimal? payMoney { get; set; }

        [Display(Name ="投资时间")]
        public DateTime payTime { get; set; }

        [Display(Name ="投资人")]
        public string sm { get; set; }

        [Display(Name = "投资人ID")]
        public string userid { get; set; }

        [Display(Name = "投资标的ID")]
        public int pjID { get; set; }
    }

    public class AmountQuery
    {

    }
}