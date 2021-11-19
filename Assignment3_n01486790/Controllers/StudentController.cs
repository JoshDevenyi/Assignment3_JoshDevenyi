using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment3_n01486790.Models;

namespace Assignment3_n01486790.Controllers
{
    public class StudentController : Controller
    {
        // GET: /Student/List
        [HttpGet]
        public ActionResult List(string SearchKey = null)
        {
            StudentDataController Controller = new StudentDataController();
            IEnumerable<Student> Students = Controller.ListStudents(SearchKey);
            return View(Students);
        }


        // GET: /Student/Show/{id}
        [HttpGet]
        [Route("Article/Show/{id}")]
        public ActionResult Show(int id)
        {
            StudentDataController Controller = new StudentDataController();
            Student SelectedStudent = Controller.FindStudent(id);

            return View(SelectedStudent);
        }

    }
}