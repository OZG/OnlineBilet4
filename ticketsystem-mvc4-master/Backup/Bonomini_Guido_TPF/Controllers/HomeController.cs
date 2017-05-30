using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonomini_Guido_TPF.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Home() //Home Page
        {
            return View();
        }

        public ActionResult UserHome() //User Home Page with User operations
        {
            return View();
        }

        public ActionResult EmployeeHome() //Employee Home Page with Employee operations
        {
            return View();
        }

        public ActionResult ManagerHome() //Manager Home Page with Manager operations
        {
            return View();
        }

    }
}
