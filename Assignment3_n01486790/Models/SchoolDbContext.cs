using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//MySQL installed in implemented 
using MySql.Data.MySqlClient;

namespace Assignment3_n01486790.Models
{
    public class SchoolDbContext
    {

        //Adding local database access information
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }


        //ConnectionString Credentials
        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }


        /// <summary>
        /// Will return a connection to the school database.
        /// </summary>
        /// <example>
        /// private SchoolDBContext School = new SchoolDbContext();
        /// MySQLCOnnection Conn = School.AccessDatabase();
        /// </example>
        /// <returns>A MySQLConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            //Creating a new object connected to the database
            return new MySqlConnection(ConnectionString);
        }

    }
}