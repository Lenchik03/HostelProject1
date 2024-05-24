using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HostelProject.mvvm.model
{
    public class AdminRepository
    {

        public AdminRepository()
        {

        }

        static AdminRepository instance;
        public static AdminRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new AdminRepository();
                return instance;
            }
        }
        // запрос на получение всех менеджеров из БД
        internal IEnumerable<Admin> GetAllManagers()
        {
            var result = new List<Admin>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT admin_id as id, FIO, user_name, password FROM admins;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Admin admin = new Admin();
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("id");
                    if (admin.Id != id)
                    {
                        admin = new Admin();
                        admin.Id = id;
                        admin.FIO = reader.GetString("FIO");
                        admin.UserName = reader.GetString("user_name");
                        admin.Password = reader.GetString("password");
                        result.Add(admin);

                    }
                }
            }

            return result;
        }

        //запрос на добавление менеджера в БД
        internal void AddManager(Admin admin)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                int id = MySqlDB.Instance.GetAutoID("admins");
                admin.Id = id;
                string sql = "INSERT INTO admins VALUES (0, @FIO, @user_name, @password)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("FIO", admin.FIO));
                    mc.Parameters.Add(new MySqlParameter("user_name", admin.UserName));
                    mc.Parameters.Add(new MySqlParameter("password", Md5Hash.HashPassword(admin.Password)));
                    mc.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // запрос на чтение таблицы менеджеров с параметром 
        //с целью получения менеджера с помощью сравнения введенных менеджером данных с имеющимися в базе данных.
        internal Admin ActiveAdmin(string username, string password)
        {
            try
            {
                Admin manager = new Admin();
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return manager;

                string sql = "SELECT admin_id as id, FIO, user_name FROM admins WHERE user_name = '" + username + "' AND password = '" + Md5Hash.HashPassword(password) + "';";

                using (var mc = new MySqlCommand(sql, connect))
                using (var reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        manager.Id = reader.GetInt32("id");
                        manager.FIO = reader.GetString("FIO");
                        manager.UserName = reader.GetString("user_name");
                    }
                }

                return manager;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
