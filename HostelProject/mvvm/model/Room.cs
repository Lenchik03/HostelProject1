using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.model
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CapacityId { get; set; }
        public int TypeId { get; set; }
        public int Del {  get; set; }

        public string CapacityTitle {  get; set; } = string.Empty;
        public int Capacity {  get; set; }
        public int PeopleCount { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
