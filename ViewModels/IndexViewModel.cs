using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel(IDialogService dialog)
        {
            TaskBars = new ObservableCollection<TaskBar>();
            CreateTaskBars();
            CreateTestData();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.dialog = dialog;
        }



        #region 属性
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<MemoDto> toDoDtos;

        public ObservableCollection<MemoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IDialogService dialog;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        #endregion


        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(); break;
                case "新增备忘录": AddMemo(); break;
            }
        }


        void AddToDo()
        {
            dialog.ShowDialog("AddToDoView");
        }

        void AddMemo()
        {
            dialog.ShowDialog("AddMemoView");
        }

        void CreateTaskBars()
        {
            taskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "9", Color = "#FF1ECA3A", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "100%", Color = "#FF02C6DC", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "2", Color = "#FFFFA000", Target = "" });
        }

        void CreateTestData()
        {
            toDoDtos = new ObservableCollection<MemoDto>();

            memoDtos = new ObservableCollection<MemoDto>();

            for (int i = 0; i < 10; i++)
            {
                toDoDtos.Add(new MemoDto() { Title = "待办" + i, Content = "正在处理中..." });
                memoDtos.Add(new MemoDto() { Title = "备忘" + i, Content = "我的密码..." });
            }
        }
    }
}
