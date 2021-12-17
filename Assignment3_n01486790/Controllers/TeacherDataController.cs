using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment3_n01486790.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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

            //To show work I spent a good chunk of time trying to figure out but didn't end up using: below is a modified query I was using to try and format the hiredate column through SQL.
            //At the time I was trying to keep HireDate in the Teacher model set to a DateTime and that seemed to cause difficulty when I was trying to use DATE_FORMAT(), but not DATE()
            //Eventually I decided to set the HireDate back to a string and used the method you'll see below (which no longer required breaking up the SELECT *).
            //I'm also assuming that using DATE_FORMAT() would have worked if HireDate was kept as a string.  

            //Experimental Query:
            //cmd.CommandText = "Select teacherid, teacherfname, teacherlname, employeenumber, DATE(hiredate) as hiredate, salary from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) " + // Broke up select all to add DATE() to hiredate
            //                  "or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) " + //Query accounts for first name, last name or both being searched. 
            //                  "order by teacherlname"; //Arranges the teacher list alphabetically

            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) " +
                              "or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) " + //Query accounts for first name, last name or both being searched. 
                              "order by teacherlname"; //Arranges the teacher list alphabetically

            //Sanatized search input added to query
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> Teachers = new List<Teacher>{}; 

            //Loop Through Each Row of the Result Set
            while (ResultSet.Read())
            {

                //Access column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();

                DateTime HireDateData = (DateTime)ResultSet["hiredate"];
                string HireDate = HireDateData.ToString("yyyy/MM/dd"); //Let me know if this was the right way to handle this.
                                                                       //I spent alot of time trying to get it working while keeping the HireData in the Teacher model as a DataTime,
                                                                       //but couldn't figure out a method to remove the unneeded timestame.

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

                DateTime HireDateData = (DateTime)ResultSet["hiredate"];
                string HireDate = HireDateData.ToString("yyyy/MM/dd"); 

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

        /// <summary>
        /// Adds a new teacher into the system
        /// </summary>
        /// <param name="NewTeacher">Teacher Object</param>
        public void AddTeacher(Teacher NewTeacher)
        {

            //Create an instance of a conneciton
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            string query = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@firstname,@lastname,@employeenumber,@hiredate,@salary)";

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@firstname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@lastname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", "T"+NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@hiredate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.Salary);


            //DML Operations
            cmd.ExecuteNonQuery();

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

        }


        /// <summary>
        /// Updates a Teacher on the MySQL database when provided teacher information (including the Teacher ID)
        /// </summary>
        /// <param name="SelectedTeacher">Teacher Information, includes the teacher's first and last names, employee number, hire date and salary</param>
        /// <returns>Nothing</returns>
        public void UpdateTeacher(Teacher SelectedTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the webserver and the daatabase
            Conn.Open();

            string query = "update teachers set teacherfname = @firstname, teacherlname = @lastname, employeenumber = @employeenumber, hiredate = @hiredate, salary = @salary where teacherid = @id";

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", SelectedTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@firstname", SelectedTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@lastname", SelectedTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@employeenumber", "T" + SelectedTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@hiredate", SelectedTeacher.HireDate);
            cmd.Parameters.AddWithValue("@salary", SelectedTeacher.Salary);

            //DML Operations
            cmd.ExecuteNonQuery();

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

        }

        /// <summary>
        /// Deletes a teacher from the database through it's primary key
        /// </summary>
        /// <param name="id">The primary key of the teacher</param>
        public void DeleteTeacher(int id) 
        {

            //Create an instance of a conneciton
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            string query = "delete from teachers where teacherid=@id";

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", id);

            //DML Operations
            cmd.ExecuteNonQuery();

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

        }

    }
}
