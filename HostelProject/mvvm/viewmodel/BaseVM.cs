using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HostelProject.mvvm.viewmodel
{
    public class BaseVM : INotifyPropertyChanged
    {// чтобы пользовательский интерфейс или другие компоненты знали, что свойство было изменено
        protected void Signal([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
