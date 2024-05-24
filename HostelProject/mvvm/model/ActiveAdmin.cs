using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.model
{
    public class ActiveAdmin
    {
        public Admin Admin { get; set; }

        public ActiveAdmin()
        {
        }

        // синглтон для получения активного юзера
        static ActiveAdmin instance;
        public static ActiveAdmin Instance
        {
            get
            {
                if (instance == null)
                    instance = new ActiveAdmin();
                return instance;
            }
        }
    }
}
