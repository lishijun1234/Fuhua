using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fuhua.Models;
using System.Data;
using CCH;


namespace Fuhua.Controllers
{
    public class AmountController : Controller
    {
        // GET: Amount
        public ActionResult Index()
        {
            ViewBag.Title = "投资明细表";
            List<AmountModels> am = SqlHelper.TableToEntity<AmountModels>(GetAmountList());
            
            return View(am);
        }

        private DataTable GetAmountList()
        {
            string s_sql = @"select pj.pjName,pj.Moneys,pj.Yearget,pj.[TimeLimit],
                            pro.payMoney,pro.payTime,u.sm
                             from paypj pj 
                            Inner Join paypro pro on pj.pjcode = pro.pjcode
                            Inner Join users u on pro.userid = u.userid
                            order by pro.payTime desc,pj.pjcode ";
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, s_sql);

            return dt;
        }
    }
}