using HostelProject.mvvm.model;
using HostelProject.mvvm.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HostelProject.mvvm.view
{
    /// <summary>
    /// Логика взаимодействия для SettingRoomPage.xaml
    /// </summary>
    public partial class SettingRoomPage : Page
    {
        public SettingRoomPage(MainVM mainVM)
        {
            InitializeComponent();
            var vm = ((SettingRoomPageVM)DataContext);
            vm.SetMainVM(mainVM);
        }
        public SettingRoomPage(MainVM mainVM, Room selectedRoom) : this(mainVM)
        {
            ((SettingRoomPageVM)DataContext).SetEditRoom(selectedRoom);
        }
    }
}
