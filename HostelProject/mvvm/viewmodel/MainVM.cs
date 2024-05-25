using HostelProject.mvvm.view;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HostelProject.mvvm.viewmodel
{
    public class MainVM : BaseVM // окно, в котором переключаются страницы
    {
        private Page currentPage;

        public static MainVM Instance { get; set; }


        public Page CurrentPage // активная страница
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }
        public VmCommand SignOut { get; set; } // кнопка "Выйти"
        public VmCommand MainPage { get; set; } // иконка Arasaka Power

        public MainVM()
        {
            Instance = this;

            CurrentPage = new SignInPage(); // по умолчанию открывается страница авторизации

            // команда на открытие страницы авторизации
            SignOut = new VmCommand(() =>
            {
                CurrentPage = new SignInPage();
            });

            // команда на открытие страницы менеджера 
            MainPage = new VmCommand(() =>
            {
                CurrentPage = new MainPage(this);
            });


        }
    }
}
