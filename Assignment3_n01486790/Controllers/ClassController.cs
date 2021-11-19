using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment3_n01486790.Models;

namespace Assignment3_n01486790.Controllers
{
    public class ClassController : Controller
    {
        // GET: /Class/List
        [HttpGet]
        public ActionResult List(string SearchKey = null)
        {
            ClassDataController Controller = new ClassDataController();
            IEnumerable<Class> Classes = Controller.ListClasses(SearchKey);
            return View(Classes);
        }


        // GET: /Class/Show/{id}
        [HttpGet]
        [Route("Class/Show/{id}")]
        public ActionResult Show(int id)
        {
            ClassDataController Controller = new ClassDataController();
            Class SelectedClass = Controller.FindClass(id);
            return View(SelectedClass);
        }

    }
}