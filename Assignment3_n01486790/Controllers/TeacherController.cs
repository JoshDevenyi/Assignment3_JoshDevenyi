using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment3_n01486790.Models;
using System.Diagnostics;

namespace Assignment3_n01486790.Controllers
{
    public class TeacherController : Controller
    {
        // GET: /Teacher/List
        [HttpGet]
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController Controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = Controller.ListTeachers(SearchKey);
            return View(Teachers);
        }


        // GET: /Teacher/Show/{id}
        [HttpGet]
        [Route("Teacher/Show/{id}")]
        public ActionResult Show(int id)
        {
            TeacherDataController Controller = new TeacherDataController();
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        // GET: /Teacher/New
        [HttpGet]
        [Route("Teacher/New")]
        public ActionResult New() 
        {

            return View();
        
        }

        // POST: /Teacher/Create
        [HttpPost]
        [Route("Teacher/Create")]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, string HireDate, decimal salary)
        {

            //I want to add a new teacher 

            TeacherDataController Controller = new TeacherDataController();

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = salary;

            Controller.AddTeacher(NewTeacher);

            //go back to the list of teachers
            return RedirectToAction("List");
        }

        
        //GET: DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {

            //Get information about teacher to confirm deletion
            TeacherDataController Controller = new TeacherDataController();
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);

        }


        //POST: Teacher/Delete/{id}
        public ActionResult Delete (int id) 
        {

            TeacherDataController Controller = new TeacherDataController();
            Controller.DeleteTeacher(id);

            return RedirectToAction("List");        

        }



    }
}