using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fuhua.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "本系统尚未全面上线，更多内容持续更新中，如果您有更好的想法，或者发现现有版本中的BUG，请及时联系我们。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = @"如果您在使用此BI系统时，
                有任何意见或建议，请及时联系我们。";

            return View();
        }
    }
}