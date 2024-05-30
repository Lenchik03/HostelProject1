using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using Type = HostelProject.mvvm.model.Type;
using System.Windows.Media;

namespace HostelProject.mvvm.viewmodel
{
    public class MainPageVM : BaseVM // страница менеджера
    {

        public VmCommand Create { get; set; } // кнопка "Добавить клиента"
        public VmCommand Edit { get; set; } // кнопка "Редактировать клиента"
        public VmCommand Delete { get; set; } // кнопка "Удалить клиента"
        public VmCommand CreateRoom { get; set; } // кнопка "Добавить тренера"
        public VmCommand EditRoom { get; set; } // кнопка "Редактировать тренера"
        public VmCommand RemoveRoom { get; set; } // кнопка "Удалить тренера"
        public VmCommand DeleteType { get; set; } // кнопка "Удалить вид абонемента"
        public VmCommand CreateType { get; set; } // кнопка "Добавить вид абонемента"
        public VmCommand EditType { get; set; } // кнопка "Редактировать вид абонемента"


        private MainVM mainVM;
        private string searchText = ""; // текст поиска
        private ObservableCollection<Guest> guests;
        private ObservableCollection<Guest> allguests;
        private ObservableCollection<Month> months;
        private ObservableCollection<Room> rooms;
        private ObservableCollection<model.Type> types;
        public ObservableCollection<Room> AllRooms { get; set; }
        public ObservableCollection<Room> AllAllRooms { get; set; }
        public ObservableCollection<Month> AllMonth { get; set; }// список тренеров для фильтра
        

        private Room selectedRoom;


        public Room SelectedRoom // выбранный из фильтра тренер, а также выбранный из списка тренеров тренер
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                Signal();
                Search();
                SearchMonth();
                SearchOutMonth();
            }
        }

        private Month selectedInMonth;


        public Month SelectedInMonth
        {
            get => selectedInMonth;
            set
            {
                selectedInMonth = value;
                Signal();
                SearchMonth();
            }
        }
        private Month selectedOutMonth;
        public Month SelectedOutMonth
        {
            get => selectedOutMonth;
            set
            {
                selectedOutMonth = value;
                Signal();
                SearchOutMonth();
            }
        }


        public string SearchText // текст, по которому мы ищем клиента
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
                SearchMonth();
                SearchOutMonth();
            }
        }

        public Guest SelectedGuest { get; set; } // выбранный из списка клиентов клиент
        public Type SelectedType { get; set; } // выбранный из списка клиентов клиент
        public ObservableCollection<Guest> Guests // список клиентов
        {
            get => guests;
            set
            {
                guests = value;
                Signal();
            }
        }

        public ObservableCollection<Guest> AllGuests // список клиентов
        {
            get => allguests;
            set
            {
                allguests = value;
                Signal();
            }
        }

        public ObservableCollection<Room> Rooms // список тренеров
        {
            get => rooms;
            set
            {
                rooms = value;
                Signal();
            }
        }
       
        public ObservableCollection<model.Type> Types // список видов абонемента

        {
            get => types;
            set
            {
                types = value;
                Signal();
            }
        }


        public MainPageVM()
        {
            // получение списка абонементов для фильтра
            string sql = "SELECT r.room_id, r.room_number, r.price, r.capacity_id, r.type_id, r.people_count, r.capacity, c.title as capacitytitle, t.title as type FROM rooms r, capacities c, types t WHERE r.capacity_id = c.capacity_id AND r.type_id = t.type_id AND r.del = 0;";
            AllRooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql));
            AllRooms.Insert(0, new Room { Id = 0, RoomNumber = "Все номера"});
            SelectedRoom = AllRooms[0];

            string sql3 = "SELECT r.room_id, r.room_number, r.price, r.capacity_id, r.type_id, r.people_count, r.capacity, c.title as capacitytitle, t.title as type FROM rooms r, capacities c, types t WHERE r.capacity_id = c.capacity_id AND r.type_id = t.type_id;";
            AllAllRooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql3));
            AllAllRooms.Insert(0, new Room { Id = 0, RoomNumber = "Все номера" });
            SelectedRoom = AllAllRooms[0];

            AllMonth = new ObservableCollection<Month>(MonthRepository.Instance.GetAllMonth());
            AllMonth.Insert(0, new Month { ID = 0 });
            SelectedInMonth = AllMonth[0];

            AllMonth = new ObservableCollection<Month>(MonthRepository.Instance.GetAllMonth());
            AllMonth.Insert(0, new Month { ID = 0});
            SelectedOutMonth = AllMonth[0];

            // получение списка всех клиентов
            string sql1 = "SELECT g.guest_id, g.name, g.secondname, g.phone_number, g.room_id, g.in_date, g.out_date, r.room_number as room_number FROM guests g, rooms r WHERE g.room_id = r.room_id AND g.out_date Is NULL;";
            Guests = new ObservableCollection<Guest>(GuestRepository.Instance.GetAllGuests(sql1));

            string sql2 = "SELECT g.guest_id, g.name, g.secondname, g.phone_number, g.room_id, g.in_date, g.out_date, r.room_number as room_number FROM guests g, rooms r WHERE g.room_id = r.room_id;";
            AllGuests = new ObservableCollection<Guest>(GuestRepository.Instance.GetAllGuests(sql2));

            // получение списка всех тренеро
            Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql));

            // получение списка всех видов абонемента
            Types = new ObservableCollection<model.Type>(TypeRepository.Instance.GetAllTypes());


            // команда на открытие страницы добавления клиента
            Create = new VmCommand(() =>
            {
                mainVM.CurrentPage = new SettingGuestPage(mainVM);
            });

            // команда на открытие страницы редактирования клиента, если был выбран клиент из списка
            Edit = new VmCommand(() => {
                if (SelectedGuest == null)
                    return;
                mainVM.CurrentPage = new SettingGuestPage(mainVM, SelectedGuest);
            });

            // команда на удаление клиента 
            Delete = new VmCommand(() =>
            {
                if (SelectedGuest == null)
                    return;

                if (MessageBox.Show("Выселение гостя", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GuestRepository.Instance.Remove(SelectedGuest);
                    //RoomRepository.Instance.UpdatePeopleCountMinus(SelectedRoom);
                    Guests.Remove(SelectedGuest); // удаление клиента из коллекции


                }
            });

            // команда на добаления тренера
            CreateRoom = new VmCommand(() =>
            {
                // открытие страницы добавления тренера
                mainVM.CurrentPage = new SettingRoomPage(mainVM);

            });

            // команда на редактирование тренера
            EditRoom = new VmCommand(() =>
            {
                // открытие страницы редактирования выбранного тренера
                mainVM.CurrentPage = new SettingRoomPage(mainVM, SelectedRoom);
            });

            // команда на удаление выбранного тренера
            RemoveRoom = new VmCommand(() => {
                if (SelectedRoom == null)
                    return;

                if (MessageBox.Show("Удаление номера", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    RoomRepository.Instance.Remove(SelectedRoom);
                    RoomRepository.Instance.UpdatePeopleCountMinus(SelectedRoom);

                    // обновление списка тренеров
                    Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql));
                }
            });


            // команда на удаление вида абонемента
            CreateType = new VmCommand(() =>
            {
                // открытие страницы добавления вида абонемента
                mainVM.CurrentPage = new SettingTypePage(mainVM);
            });

            // команда на редактирование выбранного вида абонемента
            EditType = new VmCommand(() => {
                if (SelectedType == null)
                    return;
                // открытие страницы редактирования вида абонемента
                mainVM.CurrentPage = new SettingTypePage(mainVM, SelectedType);
            });

            // команда на удаление выбранного вида абонемента
            DeleteType = new VmCommand(() =>
            {
                if (SelectedType == null)
                    return;

                if (MessageBox.Show("Удаление типа комнаты", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TypeRepository.Instance.Remove(SelectedType);

                    // обновление списка видов абонемента
                    Types = new ObservableCollection<model.Type>(TypeRepository.Instance.GetAllTypes());
                }
            });
        }

        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        private void Search()
        {
            // список клиентов после фильтрации или поиска
            Guests = new ObservableCollection<Guest>(
                GuestRepository.Instance.Search(SearchText, SelectedRoom));
        }
        private void SearchMonth()
        {
            // список клиентов после фильтрации или поиска
            AllGuests = new ObservableCollection<Guest>(
                GuestRepository.Instance.SearchMonth(SearchText, SelectedRoom, SelectedInMonth));
        }
        private void SearchOutMonth()
        {
            // список клиентов после фильтрации или поиска
            AllGuests = new ObservableCollection<Guest>(
                GuestRepository.Instance.SearchOutMonth(SearchText, SelectedRoom, SelectedOutMonth));
        }
    }
}
