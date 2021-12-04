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
    public class StudentDataController : ApiController
    {

        //Set up the database context class to access School MySql Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of students from a database
        /// </summary>
        /// <example>GET api/TeacherData/ListTeacher</example>
        /// <returns>
        /// A list of students objects
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudent/{SearchKey?}")]
        public List<Student> ListStudents(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            //cmd.CommandText = "Select * from Students order by studentlname"; //Used order by to arrange the students alphabetically
            cmd.CommandText = "Select * from Students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) " +
                              "or lower(concat(studentfname, ' ', studentlname)) like lower(@key) " + //Query accounts for first name, last name or both being searched. 
                              "order by studentlname"; //Arranges the students list alphabetically

            //Sanatized search input added to query
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an emtpy list of Students
            List<Student> Students = new List<Student> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Acecess column information by the DB column name as an index
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime EnrolDateData = (DateTime)ResultSet["enroldate"];
                string EnrolDate = EnrolDateData.ToString("dd/MM/yyyy");

                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate;

                //Add new students to the list
                Students.Add(NewStudent);

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of student names
            return Students;

        }



        /// <summary>
        ///  Returns a single student from the database by providing thier primarykey, studentid
        /// </summary>
        /// <param name="id">A student's id in the database</param>
        /// <returns>A Student Object</returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Students where studentid =" + id;

            //Gather Result of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create new student object
            Student SelectedStudent = new Student();

            //Loop through each row in the result set
            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime EnrolDateData = (DateTime)ResultSet["enroldate"];
                string EnrolDate = EnrolDateData.ToString("dd/MM/yyyy");

                SelectedStudent.StudentId = StudentId;
                SelectedStudent.StudentFname = StudentFname;
                SelectedStudent.StudentLname = StudentLname;
                SelectedStudent.StudentNumber = StudentNumber;
                SelectedStudent.EnrolDate = EnrolDate;

            }

            //Close the connection between MySQL Database and the WebServer
            Conn.Close();

            //Return the selected student
            return SelectedStudent;
            

        }


    }
}
