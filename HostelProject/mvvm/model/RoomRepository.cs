﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HostelProject.mvvm.model
{
    public class RoomRepository
    {
        static RoomRepository instance;
        public static RoomRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoomRepository();
                return instance;
            }
        }

        // запрос на чтение всех клиентов с БД
        internal IEnumerable<Room> GetAllRooms(string sql)
        {
            var result = new List<Room>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("room_id");
                    var room = new Room();

                    room.Id = id;
                    room.RoomNumber = reader.GetString("room_number");
                    room.Price = reader.GetDecimal("price");
                    room.CapacityId = reader.GetInt32("capacity_id");
                    room.TypeId = reader.GetInt32("type_id");
                    room.PeopleCount = reader.GetInt32("people_count");
                    room.Capacity = reader.GetInt32("capacity");
                    room.CapacityTitle = reader.GetString("capacitytitle");
                    room.Type = reader.GetString("type");

                    result.Add(room);
                }
            }
            return result;
        }

        //запрос на добавление клиента в БД
        internal void Add(Room room)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                int id = MySqlDB.Instance.GetAutoID("rooms");

                string sql = "INSERT INTO rooms VALUES (0, @room_number, @price, @capacity_id, @type_id, @people_count, @capacity, @del)";
                using (var mc = new MySqlCommand(sql, connect)) // INSERT - добавление клиентов в БД
                {
                    mc.Parameters.Add(new MySqlParameter("room_number", room.RoomNumber));
                    mc.Parameters.Add(new MySqlParameter("price", room.Price));
                    mc.Parameters.Add(new MySqlParameter("capacity_id", room.CapacityId));
                    mc.Parameters.Add(new MySqlParameter("type_id", room.TypeId));
                    mc.Parameters.Add(new MySqlParameter("people_count", room.PeopleCount));
                    mc.Parameters.Add(new MySqlParameter("capacity", room.Capacity));
                    mc.Parameters.Add(new MySqlParameter("del", room.Del));
                    mc.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на редактирование клиента в БД(кнопка "Редактировать клиента")
        internal void Update(Room room)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                
                    string sql = "UPDATE rooms SET room_number = @room_number, price = @price, capacity_id = @capacity_id, type_id = @type_id WHERE room_id = '" + room.Id + "';";
                    using (var mc = new MySqlCommand(sql, connect)) // UPDATE - обновление данных о клиенте
                    {
                        mc.Parameters.Add(new MySqlParameter("room_number", room.RoomNumber));
                        mc.Parameters.Add(new MySqlParameter("price", room.Price));
                        mc.Parameters.Add(new MySqlParameter("capacity_id", room.CapacityId));
                        mc.Parameters.Add(new MySqlParameter("type_id", room.TypeId));
                        mc.ExecuteNonQuery();
                    }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // запрос на удаление клиента из БД(кнопка "Удалить клиента")
        internal void Remove(Room room)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                // запрос на изменение tag(тренер, которого мы выбираем, помечается удаленным)
                string sql = "UPDATE rooms SET del = @del WHERE room_id = '" + room.Id + "';";

                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("del", 1)); // если "0" - не удален, если "1" - удален 
                    mc.ExecuteNonQuery();
                }
                string sql1 = "UPDATE guests SET out_date = @out_date WHERE room_id = '" + room.Id + "';";
                using (var mc = new MySqlCommand(sql1, connect)) // INSERT - добавление клиентов в БД
                {
                    mc.Parameters.Add(new MySqlParameter("out_date", DateTime.Now));
                    mc.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void UpdatePeopleCount(Room room)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;

                    string sql1 = "UPDATE rooms SET people_count = people_count + 1 WHERE room_id = '" + room.Id + "';";
                    using (var mc = new MySqlCommand(sql1, connect)) // INSERT - добавление клиентов в БД
                    {
                        mc.ExecuteNonQuery();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void UpdatePeopleCountMinus(Room room)
        {
            try
            {
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;
                if (room == null)
                {
                    MessageBox.Show("Не возможно выселить гостя");
                    return;
                }
                string sql1 = "UPDATE rooms SET people_count = @people_count WHERE room_id = '" + room.Id + "';";
                using (var mc = new MySqlCommand(sql1, connect)) // INSERT - добавление клиентов в БД
                {
                    mc.Parameters.Add(new MySqlParameter("people_count", room.PeopleCount - 1));
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
