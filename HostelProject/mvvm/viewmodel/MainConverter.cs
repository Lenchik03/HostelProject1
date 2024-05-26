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
    public class MainConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            string p = parameter.ToString();
            if (p == "MainPage" && value is MainPage) // если открыта страница менеджера - иконка видна
                return Visibility.Visible;
            if (p == "SettingGuestPage" && value is SettingGuestPage) // если открыта страница добавления/ редактирования клиента - иконка видна
                return Visibility.Visible;
            if (p == "SettingRoomPage" && value is SettingRoomPage) // если открыта страница добавления/ редактирования тренера - иконка видна
                return Visibility.Visible;
            if (p == "SettingTypePage" && value is SettingTypePage) // если открыта страница добавления/ редактирования вида абонемента - иконка видна
                return Visibility.Visible;

            return Visibility.Collapsed; // в остальных случаях скрыта

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
