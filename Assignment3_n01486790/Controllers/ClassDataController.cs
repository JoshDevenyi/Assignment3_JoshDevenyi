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
    public class ClassDataController : ApiController
    {

        //Set up the database context class to access School MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of the classes offered by the school from a database
        /// </summary>
        /// <example>GET api/ClassData/ListClasses</example>>
        /// <returns>
        /// A list of class objects
        /// </returns>
        [HttpGet]
        [Route("api/ClassData/ListClasses/{SearchKey?}")]
        public List<Class> ListClasses(string SearchKey=null)
        {
            //Create an instance of a conneciton
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new commnad (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL Query
            cmd.CommandText = "Select * from classes " +
                              "join teachers on classes.teacherid = teachers.teacherid " + //Joined tables to make teacher data available
                              "where lower(classname) like lower(@key) or lower(classcode) like lower(@key) " +
                              "or lower(concat(classcode, ' ', classname)) like lower(@key) or lower(concat(classcode, ': ', classname)) like lower(@key)"; //Query Accounts for the class name, class code or both (with or without a : inbetween) being searched 

            //Sanatized search input added to query
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of (School) Classes
            List<Class> Classes = new List<Class> { };

            //Loop Thorugh Each Row of the Result Set
            while (ResultSet.Read())
            {
                //Access column information by the DB column name as an index
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassName = ResultSet["classname"].ToString();
                string ClassCode = ResultSet["classcode"].ToString();
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string StartDate = ResultSet["startdate"].ToString();
                StartDate = StartDate.Substring(0, 10); //Removes the unnecessary time information from the startdate SQL data. 
                string FinishDate = ResultSet["finishdate"].ToString();
                FinishDate = FinishDate.Substring(0, 10); //Removes the unnecessary time information from the finishdate SQL data. 

                Class NewClass = new Class();
                NewClass.ClassId = ClassId;
                NewClass.ClassName = ClassName;
                NewClass.ClassCode = ClassCode;
                NewClass.TeacherId = TeacherId;
                NewClass.TeacherFname = TeacherFname;
                NewClass.TeacherLname = TeacherLname;
                NewClass.StartDate = StartDate;
                NewClass.FinishDate = FinishDate;

                //Add new classes to the list
                Classes.Add(NewClass);

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of class names
            return Classes;


        }

        /// <summary>
        /// Returns a single (school) class from the database by providing their primarykey, classid
        /// </summary>
        /// <param name="id">A class's id in the database</param>
        /// <returns>A Class Object</returns>
        [HttpGet]
        [Route("api/ClassData/FindClass/{id}")]
        public Class FindClass (int id)
        {

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and the database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Classes join teachers on classes.teacherid = teachers.teacherid where classid = " + id; //Joined tables to access teacher data

            //Gather Result of Query into a varaible
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create new class object
            Class SelectedClass = new Class();

            //Loop through each row in the result set
            while (ResultSet.Read())
            {
                //Access column information by DB column name as an index
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassName = ResultSet["classname"].ToString();
                string ClassCode = ResultSet["classcode"].ToString();
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string StartDate = ResultSet["startdate"].ToString();
                StartDate = StartDate.Substring(0, 10); //Removes the unnecessary time information from the startdate SQL data. 
                string FinishDate = ResultSet["finishdate"].ToString();
                FinishDate = FinishDate.Substring(0, 10); //Removes the unnecessary time information from the finishdate SQL data.

                SelectedClass.ClassId = ClassId;
                SelectedClass.ClassName = ClassName;
                SelectedClass.ClassCode = ClassCode;
                SelectedClass.TeacherId = TeacherId;
                SelectedClass.TeacherFname = TeacherFname;
                SelectedClass.TeacherLname = TeacherLname;
                SelectedClass.StartDate = StartDate;
                SelectedClass.FinishDate = FinishDate;

            }

            //Close the conneciton between MySQL Database and the WebServer
            Conn.Close();


            //Return the selected class
            return SelectedClass;
        } 
        
    }
}
