using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fuhua.Models;

namespace Fuhua.Controllers
{
    public class AmountController : Controller
    {
        // GET: Amount
        public ActionResult Index()
        {
            AmountModels am = new AmountModels();
           

            return View();
        }
    }
}