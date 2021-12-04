using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment3_n01486790.Models;

namespace Assignment3_n01486790.Controllers
{
    public class CourseController : Controller
    {
        // GET: /Course/List
        [HttpGet]
        public ActionResult List(string SearchKey = null)
        {
            CourseDataController Controller = new CourseDataController();
            IEnumerable <Course> Courses = Controller.ListCourses(SearchKey);
            return View(Courses);
        }


        // GET: /Course/Show/{id}
        [HttpGet]
        [Route("Course/Show/{id}")]
        public ActionResult Show(int id)
        {
            CourseDataController Controller = new CourseDataController();
            Course SelectedCourse = Controller.FindCourse(id);
            return View(SelectedCourse);
        }

    }
}