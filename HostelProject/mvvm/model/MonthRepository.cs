using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.model
{
    public class MonthRepository
    {
        static MonthRepository instance;
        public static MonthRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new MonthRepository();
                return instance;
            }
        }
        internal IEnumerable<Month> GetAllMonth()
        {
            var result = new List<Month>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT * FROM months;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    id = reader.GetInt32("month_id");
                    var month = new Month();

                    month.ID = id;

                    result.Add(month);
                }
            }
            return result;
        }
    }
}
