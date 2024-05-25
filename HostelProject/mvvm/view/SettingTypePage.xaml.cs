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
using Type = HostelProject.mvvm.model.Type;

namespace HostelProject.mvvm.view
{
    /// <summary>
    /// Логика взаимодействия для SettingTypePage.xaml
    /// </summary>
    public partial class SettingTypePage : Page
    {
        public SettingTypePage(MainVM mainVM)
        {
            InitializeComponent();
            var vm = ((SettingTypePageVM)DataContext);
            vm.SetMainVM(mainVM);
        }
        public SettingTypePage(MainVM mainVM, Type selectedType) : this(mainVM)
        {
            ((SettingTypePageVM)DataContext).SetEditType(selectedType);
        }
    }
}
