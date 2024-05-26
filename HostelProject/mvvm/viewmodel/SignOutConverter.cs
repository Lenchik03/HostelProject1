using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using HostelProject.mvvm.view;

namespace HostelProject.mvvm.viewmodel
{
    public class SignOutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            string p = parameter.ToString();
            if (p == "MainPage" && value is MainPage) // если открыта страница менеджера - кнопка "Выйти" видна
                return Visibility.Visible;

            return Visibility.Collapsed; // в остальных случаях не видна
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
