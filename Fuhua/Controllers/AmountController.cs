using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fuhua.Models;
using System.Data;
using System.Data.SqlClient;
using CCH;
using Webdiyer.WebControls.Mvc;
using System.Text;

namespace Fuhua.Controllers
{
    public class AmountController : Controller
    {
        // GET: Amount
       // [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "投资明细表";
            List<AmountModels> am = SqlHelper.TableToEntity<AmountModels>(GetAmountList());
            
            return View(am);
        }

        //获取列表
        public ActionResult List(int? pjID = null,int? Yearget = null,int? TimeLimit=null)
        {
            List<AmountModels> am = new List<AmountModels>();

            am = SqlHelper.TableToEntity<AmountModels>(GetAmountList(Yearget, TimeLimit,pjID));
            
            return View(am);
        }

        private DataTable GetAmountList()
        {
            string s_sql = @"select pj.pjName,pj.Moneys,pj.Yearget,pj.[TimeLimit],
                            pro.payMoney,pro.payTime,u.sm,u.userid,pj.pjID
                             from paypj pj 
                            Inner Join paypro pro on pj.pjcode = pro.pjcode
                            Inner Join users u on pro.userid = u.userid
                            order by pro.payTime desc,pj.pjcode ";
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, s_sql);

            return dt;
        }
        
        private DataTable GetAmountList(int? Yearget = null,int? TimeLimit=null,int? pjID = null)
        {
            StringBuilder s_sql = new StringBuilder(@"select pj.pjName,pj.Moneys,pj.Yearget,pj.[TimeLimit],
                            pro.payMoney,pro.payTime,u.sm,u.userid,pjID
                             from paypj pj 
                            Inner Join paypro pro on pj.pjcode = pro.pjcode
                            Inner Join users u on pro.userid = u.userid
                               Where 1 = 1 ");
            //order by pro.payTime desc,pj.pjcode ";

            SqlParameter[] p = new SqlParameter[]
            {
                 new SqlParameter("@Yearget",SqlDbType.Int,4),
                 new SqlParameter("@TimeLimit",SqlDbType.Int,4),
                 new SqlParameter("@ID",SqlDbType.Int,4)
            };
            p[0].Value = System.DBNull.Value;
            p[1].Value = System.DBNull.Value;
            p[2].Value = System.DBNull.Value;
            //StringBuilder sb_sqlwhere = new StringBuilder();
            if (Yearget != null)
            {
                s_sql.Append(" and pj.Yearget = @Yearget");
                p[0].Value = Yearget;
            }

            if (TimeLimit != null)
            {
                s_sql.Append(" and pj.[TimeLimit] = @TimeLimit");
                p[1].Value = TimeLimit;
            }
            if (pjID != null)
            {
                s_sql.Append(" and pj.[pjID] = @ID");
                p[2].Value = pjID;
            }

            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, s_sql.ToString(), p);

            return dt;
        }

    }
}