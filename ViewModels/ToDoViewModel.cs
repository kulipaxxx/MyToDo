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
            AddCommand = new DelegateCommand(Add);
            CreateToDoList();
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
            var todoResult = await service.GetAllAsync(new Shared.Parameters.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
            });
            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach(var todoDto in todoResult.Result.Items)
                {
                    ToDoDtos.Add(todoDto);
                }
            }
        }

    }
}
