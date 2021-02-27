using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace studentSolutions
{

    /*
      in this class we wil create the connection btw our app and the mysql db
    we need to add the mysql connector to the project
    we need to create the database 
     */
    class MY_DB
    {
        // the connection 
        private MySqlConnection conn = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=csharp_student_db");

        // create a function to get the connection
        public MySqlConnection getConnection
        {
            get
            {
                return conn;
            }
        }

        // creat a funvtion to open the connection
        public void openConnection() { 
            if(conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        // creat a funvtion to close the connection
        public void closeConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
