using MyToDo.Api.Service;
using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : BindableBase
    {
        public ToDoViewModel(IToDoService service)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            CreateToDoList();
            AddCommand = new DelegateCommand(Add);
            this.service = service;
        }

        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        public DelegateCommand AddCommand { get; private set; }

        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService service;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }//通知更新 
        }

        async void CreateToDoList()
        {
            for (int i = 0; i < 10; i++) { 
                toDoDtos.Add(new ToDoDto {  Id = i ,Content = "111"});
            }
        }

    }
}
