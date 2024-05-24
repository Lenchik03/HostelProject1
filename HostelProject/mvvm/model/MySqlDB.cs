using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HostelProject.mvvm.model
{
    public class MySqlDB
    {
        MySqlConnection mySqlConnection;

        private MySqlDB()
        {
            MySqlConnectionStringBuilder stringBuilder = new();
            stringBuilder.UserID = "student";
            stringBuilder.Password = "student";
            stringBuilder.Database = "sportClubdb";
            stringBuilder.Server = "192.168.200.13";
            stringBuilder.CharacterSet = "utf8mb4";
            mySqlConnection = new MySqlConnection(stringBuilder.ToString());
            OpenConnection();

            //MySqlConnectionStringBuilder stringBuilder = new();
            //stringBuilder.UserID = "root";
            //stringBuilder.Password = "FktyfYF03065005";
            //stringBuilder.Database = "sportclubdb1";
            //stringBuilder.Server = "localhost";
            //stringBuilder.CharacterSet = "utf8mb4";
            //MySqlConnection = new MySqlConnection("server=192.168.200.13;user=student;password=student;database=tasksDB;Character Set=utf8mb4");
            //mySqlConnection = new MySqlConnection(stringBuilder.ToString());
            //OpenConnection();
        }

        // открытие соединения с БД
        private bool OpenConnection()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        // закрытие соединения с БД
        public void CloseConnection()
        {
            try
            {
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // проверка на открытость соединения
        internal MySqlConnection GetConnection()
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                if (!OpenConnection())
                    return null;

            return mySqlConnection;
        }

        static MySqlDB instance;
        public static MySqlDB Instance
        {
            get
            {
                if (instance == null)
                    instance = new MySqlDB();
                return instance;
            }
        }

        // для автоматической нумерации новых клиентов\тренеров\видов абонемента
        public int GetAutoID(string table)
        {
            try
            {
                string sql = "SHOW TABLE STATUS WHERE `Name` = '" + table + "'";
                using (var mc = new MySqlCommand(sql, mySqlConnection))
                using (var reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32("Auto_increment");
                }
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
    }
}
