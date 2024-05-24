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
        private ObservableCollection<Room> rooms;
        private ObservableCollection<model.Type> types;
        public ObservableCollection<Room> AllRooms { get; set; } // список тренеров для фильтра
        
        private Room selectedRoom;


        public Room SelectedRoom // выбранный из фильтра тренер, а также выбранный из списка тренеров тренер
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                Signal();
                Search();
            }
        }
        

        public string SearchText // текст, по которому мы ищем клиента
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
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
            
            AllRooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms());
            AllRooms.Insert(0, new Room { Id = 0, RoomNumber = 0});
            SelectedRoom = AllRooms[0];


            // получение списка всех клиентов
            string sql = "SELECT g.name, g.secondname, g.room_id, g.in_date, g.out_date, r.room_number as room_number FROM guests g, rooms r WHERE g.room_id = r.room_id;";
            Guests = new ObservableCollection<Guest>(GuestRepository.Instance.GetAllGuests(sql));

            // получение списка всех тренеров
            string sql1 = "SELECT c.coachID as id, c.FIO, c.typeActivitiesID, c.phone_number, t.title as typeActivities from coaches c, typeActivities t where c.typeActivitiesID = t.typeActivitiesID and c.tag = 0 ORDER BY id;";
            Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms());

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
            Delete = new VmCommand(() => {
                if (SelectedGuest == null)
                    return;

                if (MessageBox.Show("Удаление гостя", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GuestRepository.Instance.Remove(SelectedGuest);
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

                if (MessageBox.Show("Удаление тренера", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    RoomRepository.Instance.Remove(SelectedRoom);

                    // обновление списка тренеров
                    Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms());
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
    }
}
