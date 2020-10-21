using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace ClientSide
{
    public class DBclient
    {
        System.Data.SQLite.SQLiteConnection m_dbConnection;
        private String DB = "triggersTable.sqlite";
      
        public DBclient()
        {
          
            /* createNewDatabase();
             connectToDatabase();
             createTable();
             fillTable();*/
        }

        public void createNewDatabase()
        {
            SQLiteConnection.CreateFile(DB);
        }

        // Creates a connection with our database file.
        public void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + DB + ";Version=3;");
            m_dbConnection.Open();
        }

        // Creates a table named 'clientData' with two columns: name (a string of max 20 characters) and score (an int)
        public void createTable()
        {
            //string sql = "create table clientData (name varchar(20), settingString varchar(20))";
            string sql = "create table triggersTable(idTriger INTEGER ,dateTriger TEXT, DesTriger TEXT)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

        }

        // Inserts some values in the clientData table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        public void fillTable(int idTriger, string dateTriger, string DesTriger)
        {
            string sql = "insert into triggersTable (idTriger,dateTriger,DesTriger) values('" + idTriger + "','" + dateTriger + "','" + DesTriger + "');";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            /* string sql = "insert into clientData (name, score) values ('Me', 3000)";
             SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
             command.ExecuteNonQuery();
             sql = "insert into clientData (name, score) values ('Myself', 6000)";
             command = new SQLiteCommand(sql, m_dbConnection);
             command.ExecuteNonQuery();
             sql = "insert into clientData (name, score) values ('And I', 9001)";
             command = new SQLiteCommand(sql, m_dbConnection);
             command.ExecuteNonQuery();*/
        }

        // Writes the clientData to the console sorted on score in descending order.
        void printClientData()
        {
            string  Text = "";
             string sql = "select * from  triggersTable order by idTriger";
             SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
             SQLiteDataReader reader = command.ExecuteReader();
             while (reader.Read())
             {
                 Text += "id: " + reader["idTriger"] + "date: " + reader["dateTriger"] + "\tdescription: " + reader["DesTriger"] + "\n";
             }

             //m_dbConnection.Close();
         }
        
    }
}

