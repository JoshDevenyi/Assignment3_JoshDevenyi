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


        //This request will render a page with an update form given a Teacher's ID
        // GET: /Teacher/Update/{id}
        [HttpGet]
        [Route("Teacher/Update/{id}")]
        public ActionResult Update(int id)
        {
      
            TeacherDataController Controller = new TeacherDataController();
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);

        }


        //This request will actually update a teacher's information
        // POST: /Teacher/Update/{id}
        [HttpPost]
        [Route("Teacher/Update/{id}")]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber,  string HireDate, decimal salary)
        {

            TeacherDataController Controller = new TeacherDataController();

            Teacher SelectedTeacher = new Teacher();
            SelectedTeacher.TeacherId = id;
            SelectedTeacher.TeacherFname = TeacherFname;
            SelectedTeacher.TeacherLname = TeacherLname;
            SelectedTeacher.EmployeeNumber = EmployeeNumber;
            SelectedTeacher.HireDate = HireDate;
            SelectedTeacher.Salary = salary;

            Controller.UpdateTeacher(SelectedTeacher);

            return RedirectToAction("Show/" + id);
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