using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HostelProject.mvvm.model
{
    public class TypeRepository
    {
        static TypeRepository instance;
        public static TypeRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new TypeRepository();
                return instance;
            }
        }

        // запрос на чтение всех клиентов с БД
        internal IEnumerable<Type> GetAllTypes()
        {
            var result = new List<Type>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT type_id, title FROM types;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("type_id");
                    var type = new Type();

                    type.Id = id;
                    type.Title = reader.GetString("title");

                    result.Add(type);
                }
            }
            return result;
        }

        //запрос на добавление клиента в БД
        internal void Add(Type type)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                int id = MySqlDB.Instance.GetAutoID("types");

                string sql = "INSERT INTO types VALUES (0, @title, @del)";
                using (var mc = new MySqlCommand(sql, connect)) // INSERT - добавление клиентов в БД
                {
                    mc.Parameters.Add(new MySqlParameter("title", type.Title));
                    mc.Parameters.Add(new MySqlParameter("del", type.Del));
                    mc.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на редактирование клиента в БД(кнопка "Редактировать клиента")
        internal void Update(Type type)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;


                string sql = "UPDATE types SET title = @title WHERE type_id = '" + type.Id + "';";
                using (var mc = new MySqlCommand(sql, connect)) // UPDATE - обновление данных о клиенте
                {
                    mc.Parameters.Add(new MySqlParameter("title", type.Title));
                    mc.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на удаление клиента из БД(кнопка "Удалить клиента")
        internal void Remove(Type type)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                // запрос на изменение tag(тренер, которого мы выбираем, помечается удаленным)
                string sql = "UPDATE rooms SET del = @del WHERE room_id = '" + type.Id + "';";

                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("del", 1)); // если "0" - не удален, если "1" - удален 
                    mc.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
