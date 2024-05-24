using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostelProject.mvvm.model;
using Type = HostelProject.mvvm.model.Type;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingTypePageVM: BaseVM
    {
        public VmCommand Save { get; set; } // кнопка "Сохранить"

        MainVM mainVM;


        private Type type = new();


        public Type Type // вид абонемента, который мы добавляем или редактируем
        {
            get => type;
            set
            {
                type = value;
                Signal();
            }
        }
        public SettingTypePageVM()
        {
            //команда на добавление в базу или обновление вида абонемента в базе
            Save = new VmCommand(() =>
            {
                Type.Del = 0;
                if (Type.Id == 0)
                {
                    TypeRepository.Instance.Add(Type); // добавление вида абонемента

                }
                else
                {
                    // редактирование выбранного вида абонемента
                    TypeRepository.Instance.Update(Type);
                }

                // после успешного добавления или редактирования вида абонемента откроется страница менеджера
                mainVM.CurrentPage = new MainPageVM(mainVM);
            });


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditTA(Type type)
        {
            Type = type;

        }
    }
}
