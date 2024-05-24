using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MySqlConnector;

namespace HostelProject.mvvm.model
{
    public class GuestRepository
    {
        static GuestRepository instance;
        public static GuestRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new GuestRepository();
                return instance;
            }
        }

        // запрос на чтение всех клиентов с БД
        internal IEnumerable<Guest> GetAllGuests(string sql)
        {
            var result = new List<Guest>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("guest_id");
                    var guest = new Guest();

                    guest.Id = id;
                    guest.Name = reader.GetString("name");
                    guest.SecondName = reader.GetString("secondname");
                    guest.Name = reader.GetString("phone_number");
                    guest.RoomId = reader.GetInt32("room_id");
                    guest.InDate = reader.GetDateTime("in_date");
                    guest.OutDate = reader.GetDateTime("out_date");
                    guest.RoomNumber = reader.GetInt32("room_number");

                    result.Add(guest);
                }
            }
            return result;
        }

        //запрос на добавление клиента в БД
        internal void Add(Guest guest)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                int id = MySqlDB.Instance.GetAutoID("clients");

                string sql = "INSERT INTO guests VALUES (0, @name, @secondname, @phone_number, @room_id, @out_date)";
                using (var mc = new MySqlCommand(sql, connect)) // INSERT - добавление клиентов в БД
                {
                    mc.Parameters.Add(new MySqlParameter("name", guest.Name));
                    mc.Parameters.Add(new MySqlParameter("secondname", guest.SecondName));
                    mc.Parameters.Add(new MySqlParameter("phone_number", guest.PhoneNumber));
                    mc.Parameters.Add(new MySqlParameter("room_id", guest.RoomId));
                    mc.Parameters.Add(new MySqlParameter("out_date", guest.OutDate));
                    mc.ExecuteNonQuery();
                }

                //string sql1 = "UPDATE rooms SET people_count = people_count + 1;";
                //using (var mc = new MySqlCommand(sql1, connect)) // INSERT - добавление клиентов в БД
                //{
                //    mc.ExecuteNonQuery();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на редактирование клиента в БД(кнопка "Редактировать клиента")
        internal void Update(Guest guest)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;


                string sql = "UPDATE guests SET name = @name, secondname = @secondname, room_id = @room_id, out_date = @out_date WHERE guest_id = '" + guest.Id + "';";
                using (var mc = new MySqlCommand(sql, connect)) // UPDATE - обновление данных о клиенте
                {
                    mc.Parameters.Add(new MySqlParameter("name", guest.Name));
                    mc.Parameters.Add(new MySqlParameter("secondname", guest.SecondName));
                    mc.Parameters.Add(new MySqlParameter("room_id", guest.RoomId));
                    mc.Parameters.Add(new MySqlParameter("out_date", guest.OutDate));
                    mc.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на удаление клиента из БД(кнопка "Удалить клиента")
        internal void Remove(Guest guest)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                string sql = "DELETE FROM guests WHERE guest_id = '" + guest.Id + "';"; // DELETE - удаление клиента из БД 

                using (var mc = new MySqlCommand(sql, connect))
                    mc.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на выборку клиентов(поиск клиента) по параметам ФИО клиента и номера телефона клиента
        // + запрос на фильтрацию по ФИО тренера, по названию абонемента, по виду абонемента
        internal IEnumerable<Guest> Search(string searchText, Room selectedRoom)
        {
            try
            {
                string sql = "SELECT g.name, g.secondname, g.room_id, g.in_date, g.out_date, r.room_number as room_number FROM guests g, rooms r WHERE g.room_id = r.room_id";
                sql += " AND( g.secondname LIKE '%" + searchText + "%'";
                sql += " OR g.phone_number LIKE '%" + searchText + "%')";
                if (selectedRoom != null && selectedRoom.Id != 0)
                    sql += " AND g.room_id = " + selectedRoom.Id;
                sql += " ORDER BY c.clientID;";

                return GetAllGuests(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
