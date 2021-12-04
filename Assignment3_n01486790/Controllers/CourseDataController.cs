using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment3_n01486790.Models;
using MySql.Data.MySqlClient;

namespace Assignment3_n01486790.Controllers
{
    public class CourseDataController : ApiController
    {

        //Set up the database context class to access School MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of the coursees offered by the school from a database
        /// </summary>
        /// <example>GET api/CourseData/ListCourses</example>>
        /// <returns>
        /// A list of course objects
        /// </returns>
        [HttpGet]
        [Route("api/CourseData/ListCourses/{SearchKey?}")]
        public List<Course> ListCourses(string SearchKey=null)
        {
            //Create an instance of a conneciton
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new commnad (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from classes " +
                              "left join teachers on classes.teacherid = teachers.teacherid " + //Joined tables to make teacher data available
                              "where lower(classname) like lower(@key) or lower(classcode) like lower(@key) " +
                              "or lower(concat(classcode, ' ', classname)) like lower(@key) or lower(concat(classcode, ': ', classname)) like lower(@key)"; //Query Accounts for the course name, course code or both (with or without a : inbetween) being searched 

            //Sanatized search input added to query
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Courses
            List<Course> Courses = new List<Course> { };

            //Loop Thorugh Each Row of the Result Set
            while (ResultSet.Read())
            {
                //Access column information by the DB column name as an index
                int CourseId = Convert.ToInt32(ResultSet["classid"]);
                string CourseName = ResultSet["classname"].ToString();
                string CourseCode = ResultSet["classcode"].ToString();


                //Added this if statement to fix a DBNull error the new "left join" introduced when I added a teacher-less course to the database. Let me know if there is a better solution. 
                int TeacherId = 0;
                if (ResultSet["teacherid"] != DBNull.Value)
                {
                    TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                }

                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();


                //Added this section to fix a DBNull error the new "left join" introduced
                DateTime StartDateData; 
                DateTime FinishDateData;
                string StartDate = "";
                string FinishDate = "";

                if (ResultSet["startdate"] != DBNull.Value)
                {
                   StartDateData = (DateTime)ResultSet["startdate"];
                    StartDate = StartDateData.ToString("dd/MM/yyyy");
                }

                if (ResultSet["finishdate"] != DBNull.Value)
                {
                    FinishDateData = (DateTime)ResultSet["finishdate"];
                    FinishDate = FinishDateData.ToString("dd/MM/yyyy");
                }

                Course NewCourse = new Course();
                NewCourse.CourseId = CourseId;
                NewCourse.CourseName = CourseName;
                NewCourse.CourseCode = CourseCode;
                NewCourse.TeacherId = TeacherId;
                NewCourse.TeacherFname = TeacherFname;
                NewCourse.TeacherLname = TeacherLname;
                NewCourse.StartDate = StartDate;
                NewCourse.FinishDate = FinishDate;

                //Add new classes to the list
                Courses.Add(NewCourse);

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of course names
            return Courses;


        }

        /// <summary>
        /// Returns a single (school) course from the database by providing their primarykey, classid
        /// </summary>
        /// <param name="id">A course's id in the database</param>
        /// <returns>A Course Object</returns>
        [HttpGet]
        [Route("api/CourseData/FindCourse/{id}")]
        public Course FindCourse (int id)
        {

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Classes left join teachers on classes.teacherid = teachers.teacherid where classid = " + id; //Joined tables to access teacher data

            //Gather Result of Query into a varaible
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create new course object
            Course SelectedCourse = new Course();

            //Loop through each row in the result set
            while (ResultSet.Read())
            {
                //Access column information by DB column name as an index
                int CourseId = Convert.ToInt32(ResultSet["classid"]);
                string CourseName = ResultSet["classname"].ToString();
                string CourseCode = ResultSet["classcode"].ToString();

                int TeacherId = 0;
                //Added this if statement to fix a DBNull error the new "left join" introduced when I added a teacher-less course to the database.
                if (ResultSet["teacherid"] != DBNull.Value)
                {
                    TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                }

                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();


                //Added this section to fix a DBNull error the new "left join" introduced
                DateTime StartDateData;
                DateTime FinishDateData;
                string StartDate = "";
                string FinishDate = "";

                if (ResultSet["startdate"] != DBNull.Value)
                {
                    StartDateData = (DateTime)ResultSet["startdate"];
                    StartDate = StartDateData.ToString("dd/MM/yyyy");
                }

                if (ResultSet["finishdate"] != DBNull.Value)
                {
                    FinishDateData = (DateTime)ResultSet["finishdate"];
                    FinishDate = FinishDateData.ToString("dd/MM/yyyy");
                }

                SelectedCourse.CourseId = CourseId;
                SelectedCourse.CourseName = CourseName;
                SelectedCourse.CourseCode = CourseCode;
                SelectedCourse.TeacherId = TeacherId;
                SelectedCourse.TeacherFname = TeacherFname;
                SelectedCourse.TeacherLname = TeacherLname;
                SelectedCourse.StartDate = StartDate;
                SelectedCourse.FinishDate = FinishDate;

            }

            //Close the conneciton between MySQL Database and the WebServer
            Conn.Close();


            //Return the selected class
            return SelectedCourse;
        } 
        
    }
}
