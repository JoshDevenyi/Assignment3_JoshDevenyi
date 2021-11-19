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
    public class TeacherDataController : ApiController
    {

        //Set up the database context class to access School MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of teachers from a database
        /// </summary>
        /// <example>GET api/TeacherData/ListTeacher</example>
        /// <returns>
        /// A list of teacher objects
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeacher/{SearchKey?}")]
        public List<Teacher> ListTeachers(string SearchKey=null)
        {

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY

            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) " +
                              "or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) " + //Query accounts for first name, last name or both being searched. 
                              "order by teacherlname"; //Arranges the teacher list alphabetically

            //Sanatized search input added to query
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row of the Result Set
            while (ResultSet.Read())
            {

                //Access column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                string HireDate = ResultSet["hiredate"].ToString();
                HireDate = HireDate.Substring(0, 10); //Removes the unnecessary time information from the hiredate SQL data. 
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
                

                //Add new teachers to the list
                Teachers.Add(NewTeacher);

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teacher names
            return Teachers;

        }



        /// <summary>
        /// Returns a single teacher from the database by providing their primary key, teacherid
        /// </summary>
        /// <param name="id">A teacher's id in the database</param>
        /// <returns>A Teacher Object</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where teacherid =" + id;

            //Gather Result of Query into a varaible
            MySqlDataReader ResultSet = cmd.ExecuteReader();
         
            //Create new teacher object
            Teacher SelectedTeacher = new Teacher();

            //Loop through each row in the result set
            while (ResultSet.Read()) 
            {
                //Access column information by DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                string HireDate = ResultSet["hiredate"].ToString();
                HireDate = HireDate.Substring(0, 10); //Removes the unnecessary time information from the hiredate SQL data. 
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                
                SelectedTeacher.TeacherId = TeacherId;
                SelectedTeacher.TeacherFname = TeacherFname;
                SelectedTeacher.TeacherLname = TeacherLname;
                SelectedTeacher.EmployeeNumber = EmployeeNumber;
                SelectedTeacher.HireDate = HireDate;
                SelectedTeacher.Salary = Salary;

            }

            //Close the connection between MySQL Database and the WebServer
            Conn.Close();

            //Return the selected teacher
            return SelectedTeacher;

        }


        
    }
}
