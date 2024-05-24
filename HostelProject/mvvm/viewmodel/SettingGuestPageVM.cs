using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingGuestPageVM: BaseVM
    {
        private Room selectedRoom;
        public VmCommand Save { get; set; } // кнопка "Сохранить"
        MainVM mainVM;

        private Guest guest = new();

        public Guest Guest // клиент, которого мы добавляем или редактируем
        {
            get => guest;
            set
            {
                guest = value;
                Signal();
            }
        }

        public Room SelectedRoom // выбранный тренер из ComboBox`a
        {
            get => selectedRoom; set
            {
                selectedRoom = value;
                Signal();
            }
        }
        public List<Room> AllRooms { get; set; } // список абонементов (ComboBox абонментов)

        public SettingGuestPageVM()
        {
            // получение списка абонементов
            AllRooms = (List<Room>?)RoomRepository.Instance.GetAllRooms();

            //команда на добавление в базу или обновление клиента в базе
            Save = new VmCommand(() => {

                // если из Combobox`а НЕ выбран абонемент, то по умолчанию будет выбран первый абонемент
                Guest.RoomId = SelectedRoom?.Id ?? 1;

                if (Guest.Id == 0)
                {
                    if (SelectedRoom.PeopleCount <= SelectedRoom.Capacity)
                    {
                        RoomRepository.Instance.UpdatePeopleCount(SelectedRoom);
                        GuestRepository.Instance.Add(Guest); // добавление клиента
                    }
                }
                else
                {                    
                    GuestRepository.Instance.Update(Guest); // если клиент выбран из списка - редактирование клиента

                }

                mainVM.CurrentPage = new MainPage(mainVM); // после успешного добавления или редактирования клиента, откроется страница менеджера
            });


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditClient(Guest selectedGuest)
        {
            Guest = selectedGuest;
            SelectedRoom = AllRooms.FirstOrDefault(s => s.Id == guest.Id);
        }
    }
}
