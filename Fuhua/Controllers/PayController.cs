using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using CCH;
using System.Threading.Tasks;
using System.Data;

namespace Fuhua.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        [Authorize]
        public ActionResult payMoney()
        {
            return View();
        }

        // POST: /Pay/payMoney
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult payMoney(string s_inuserid, string s_outuserid, decimal d_principalMoney, decimal d_interestMoney)
        {
            if (ModelState.IsValid)
            {
                if (existEnoughMoney(s_outuserid, d_interestMoney + d_principalMoney))
                {
                    var result = execpayMoney(s_inuserid, s_outuserid, d_principalMoney, d_interestMoney);
                    if (result)
                    {
                        ViewBag.result = "还款成功";
                        
                    }
                }
                else
                {
                    ViewBag.result = "融资人账户余额不足！";
                }
                return View();
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View();
        }

        /// <summary>
        /// 还款人偿还投资人资金的方法
        /// </summary>
        /// <param name="s_inuserid">投资人</param>
        /// <param name="s_outuserid">还款人</param>
        /// <param name="d_principalMoney">本金</param>
        /// <param name="d_interestMoney">利息</param>
        private bool execpayMoney(string s_inuserid,string s_outuserid,decimal d_principalMoney,decimal d_interestMoney)
        {
            SqlParameter[] p1 = new SqlParameter[]
            {
                new SqlParameter("@inuserid",System.Data.SqlDbType.VarChar,50),
                new SqlParameter("@outuserid",System.Data.SqlDbType.VarChar,50),
                new SqlParameter("@principalMoney",System.Data.SqlDbType.Decimal,12),
                new SqlParameter("@interestMoney",System.Data.SqlDbType.Decimal,12),
                new SqlParameter("@msg",System.Data.SqlDbType.NVarChar,100)
            };
            p1[0].Value = s_inuserid;
            p1[1].Value = s_outuserid;
            p1[2].Value = d_principalMoney;
            p1[3].Value = d_interestMoney;
            p1[4].Direction = ParameterDirection.Output;
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["execConnection"].ToString());
            int i = SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "p_payMoney", p1);

                return (Convert.ToString(p1[4].Value) == "利息计算成功，提交事务");


        }

        private bool existEnoughMoney(string s_UserID,decimal d_Money)
        {
            
            SqlParameter p1 = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            p1.Value = s_UserID;
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, "Select isnull(NowMoney,0) as NowMoney  From users where UserID = @UserID", p1);
            if(dt.Rows.Count>0)
            {
                return (d_Money <= Convert.ToDecimal(dt.Rows[0][0]));

            }
            else
            {
                return false;
            }
                
        }
    }
}