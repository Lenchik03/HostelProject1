﻿using HostelProject.mvvm.viewmodel;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage(MainVM mainVM)
        {
            InitializeComponent();
            var vm = ((RegistrationPageVM)DataContext);
            vm.SetMainVM(mainVM);
            vm.SetPasswordBox(passwrdBox);
        }
    }
}
