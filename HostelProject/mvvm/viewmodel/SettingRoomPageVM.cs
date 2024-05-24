using HostelProject.mvvm.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Type = HostelProject.mvvm.model.Type;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingRoomPageVM: BaseVM
    {
        private Capacity selectedCapacity;
        private Type selectedType;
        public VmCommand Save { get; set; } // кнопка "Сохранить"
        MainVM mainVM;

        private Room room = new();

        public Room Room // клиент, которого мы добавляем или редактируем
        {
            get => room;
            set
            {
                room = value;
                Signal();
            }
        }

        public Capacity SelectedCapacity // выбранный абонемент из ComboBox`a
        {
            get => selectedCapacity; set
            {
                selectedCapacity = value;
                Signal();
            }
        }
        public Type SelectedType // выбранный вид абонемента из ComboBox`a
        {
            get => selectedType; set
            {
                selectedType = value;
                Signal();
            }
        }

        
        public List<Capacity> AllCapacity { get; set; } // список абонементов (ComboBox абонментов)
        public List<Type> AllType { get; set; } // список видов абонемента (ComboBox видов)

        public SettingRoomPageVM()
        {
            // получение списка абонементов
            AllCapacity = (List<Capacity>?)CapacityRepository.Instance.GetAllCapacities();

            // получение списка видов абонемента
            AllType = (List<Type>?)TypeRepository.Instance.GetAllTypes();


            //команда на добавление в базу или обновление клиента в базе
            Save = new VmCommand(() => {

                // если из Combobox`а НЕ выбран абонемент, то по умолчанию будет выбран первый абонемент
                Room.CapacityId = SelectedCapacity?.Id ?? 1;

                // если из Combobox`а НЕ выбран вид абонемента, то по умолчанию будет выбран первый вид
                Room.TypeId = SelectedType?.Id ?? 1;

                Room.Del = 0; // по умолчанию тренер не удален
                
                if (Room.Id == 0)
                {
                    if(Room.CapacityId == 1)
                        Room.Capacity = 1;
                    else if (Room.CapacityId == 2)
                        Room.Capacity = 2;
                    else
                        Room.Capacity = 4;

                    RoomRepository.Instance.Add(Room); // добавление клиента

                }
                else
                {
                    RoomRepository.Instance.Update(Room); // если клиент выбран из списка - редактирование клиента

                }

                mainVM.CurrentPage = new MainPageVM(mainVM); // после успешного добавления или редактирования клиента, откроется страница менеджера
            });


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditClient(Room selectedRoom)
        {
            Room = selectedRoom;
            SelectedCapacity = AllCapacity.FirstOrDefault(s => s.Id == room.CapacityId);
            SelectedType = AllType.FirstOrDefault(s => s.Id == room.TypeId);
        }
    }
}
