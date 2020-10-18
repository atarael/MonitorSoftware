using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace ServerSide
{
    public class DBserver
    {
         System.Data.SQLite.SQLiteConnection m_dbConnection;
        private String DB = "";
        public DBserver(string db)
        {
            this.DB = db + ".sqlite";
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
            string sql = "create table clientData(id INTEGER PRIMARY KEY,name TEXT, settingString TEXT)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

        }

        // Inserts some values in the clientData table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        public void fillTable(int id1, string name1, string settingString1)
        {
            string sql = "insert into clientData (id,name,settingString) values('" + id1 + "','" + name1 + "','" + settingString1 + "');";

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
            /* txbDB.Text = "";
             string sql = "select * from clientData order by score desc";
             SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
             SQLiteDataReader reader = command.ExecuteReader();
             while (reader.Read())
             {
                 txbDB.Text += "Name: " + reader["name"] + "\tScore: " + reader["score"] + "\n";
             }

             m_dbConnection.Close();
         }*/
        }
    }
}

