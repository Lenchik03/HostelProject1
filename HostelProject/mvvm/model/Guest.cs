using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.model
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public DateTime InDate { get; set; } = DateTime.Now;
        public DateTime OutDate { get; set; }


        public int RoomNumber { get; set; } 
    }
}
