using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterCaseTracking.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult _AreaList()
        {
            return View();
        }
    }
}