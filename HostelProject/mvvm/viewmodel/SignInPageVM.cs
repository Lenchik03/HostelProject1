using HostelProject.mvvm.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using HostelProject.mvvm.model;

namespace HostelProject.mvvm.viewmodel
{
    public class SignInPageVM: BaseVM
    {
        private MainVM mainVM;
        public VmCommand Open { get; } // кнопка "Войти"
        public VmCommand Registration { get; } // кнопка "Зарегистрироваться"

        public SignInPageVM()
        {
            Open = new VmCommand(() =>
            {
                // получение активного менеджера с помощью срвнения введенных данных с имеющимеся в базе
                ActiveAdmin.Instance.Admin = AdminRepository.Instance.ActiveAdmin(UserName, Password);

                var manager = ActiveAdmin.Instance.Admin;
                if (manager.Id == 0)
                {
                    MessageBox.Show("Неверное имя пользователя или пароль");
                }

                else
                {
                    // открытие страницы менеджера
                    MainPage mainPage = new MainPage(MainVM.Instance);
                    MainVM.Instance.CurrentPage = mainPage;
                }
            });

            Registration = new VmCommand(() =>
            {
                // открытие страницы регистрации
                MainVM.Instance.CurrentPage = new RegistrationPage(MainVM.Instance);
            });
        }

        internal void SetPasswordBox(PasswordBox passwrdBox)
        {
            this.passwrdBox = passwrdBox;
            passwrdBox.PasswordChanged += EventPassword;
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        private void EventPassword(object sender, RoutedEventArgs e)
        {
            Signal(nameof(Password));
        }

        private string username;
        public string UserName // введенный менеджером Email для входа
        {
            get { return username; }
            set
            {
                username = value;
            }
        }

        private PasswordBox passwrdBox;

        public string Password // введенный менеджером пароль для входа
        {
            get { return passwrdBox.Password; }
            set
            {
                passwrdBox.Password = value;
            }
        }
    }
}
