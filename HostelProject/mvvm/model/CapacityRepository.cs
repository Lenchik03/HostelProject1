using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.model
{
    public class CapacityRepository
    {
        static CapacityRepository instance;
        public static CapacityRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new CapacityRepository();
                return instance;
            }
        }
        internal IEnumerable<Capacity> GetAllCapacities()
        {
            var result = new List<Capacity>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT capacity_id, title FROM capacities;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("capacity_id");
                    var capacity = new Capacity();

                    capacity.Id = id;
                    capacity.Title = reader.GetString("title");

                    result.Add(capacity);
                }
            }
            return result;
        }
    }
}
