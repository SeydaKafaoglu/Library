using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class LibraryControllerController : Controller
    {
        // GET: LibraryController
        public LibraryControllerController()
        {
            if (System.Web.HttpContext.Current.Session["userId"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/home/login");
            }
  

        }
    }
}