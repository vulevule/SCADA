using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace SCADA_Core
{
    public class DBManager
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string pass;

        public DBManager()
        {
            server = "localhost";
            database = "snus_shema";
            uid = "root";
            pass = "jaciodgadafija";

            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();

            connBuilder.Add("Database", database);
            connBuilder.Add("Data Source", server);
            connBuilder.Add("User Id", uid);
            connBuilder.Add("Password", pass);

            connection = new MySqlConnection(connBuilder.ConnectionString);

        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                Debug.Write($"Can not connect to the server. Reason: {e.Message}");
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.Write($"Can not close connection. Reason: {e.Message}");
                return false;
            }
        }
        public void Insert(Measurement m)
        {
            string query = "INSERT INTO measurement (tagID, value, time)  VALUES (@id, @value, CURRENT_TIMESTAMP)";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@id", m.TagId);
                cmd.Parameters.AddWithValue("@value", m.Value);

                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void Clear()
        {
            string query = "DELETE FROM measurement";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.ExecuteNonQuery();

                this.CloseConnection();
            }
        }


        public List<Measurement> Select()
        {
            string query = "SELECT * FROM measurement ORDER BY time ASC";

            List<Measurement> data = new List<Measurement>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Measurement m = new Measurement() { TagId = dataReader.GetString(0), Value = dataReader.GetDouble(1), Time = dataReader.GetDateTime(2) };
                    data.Add(m);

                }

                dataReader.Close();

                this.CloseConnection();

                return data;

            }
            else
            {
                return data;
            }
        }

    }
}