
using MyToDo.API.Context;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
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
    public class IndexViewModel : NavigationViewModel
    {


        public IndexViewModel(IContainerProvider provider
            ,IDialogHostService dialog) : base(provider)
        {
            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();

            CreateTaskBars();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            
            this.toDoService =  provider.Resolve<IToDoService>();
            this.memoService =  provider.Resolve<IMemoService>();
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            ToDoCompleteCommand = new DelegateCommand<ToDoDto>(Complete);
            this.dialog = dialog;
        }



        #region 属性

        private readonly IToDoService toDoService;

        private readonly IMemoService memoService;

        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand<ToDoDto> ToDoCompleteCommand {  get; private set; }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IDialogHostService dialog;

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
                case "新增待办": AddToDo(null); break;
                case "新增备忘录": AddMemo(null); break;
            }
        }


        /// <summary>
        /// 添加待办事项
        /// </summary>
        async void AddToDo(ToDoDto model)
        {
            DialogParameters parameters = new DialogParameters();

            if(model != null)
            {
                parameters.Add("value", model);
            }

            var dialogResult = await dialog.ShowDialog("AddToDoView", parameters);
            if(dialogResult.Result == ButtonResult.OK)
            {
                var todo = dialogResult.Parameters.GetValue<ToDoDto>("value");

                if(todo.Id > 0)
                {
                    var updateResult = await toDoService.UpdateAsync(todo);

                    if (updateResult.Status)
                    {
                        var todoModel = ToDoDtos.FirstOrDefault(t => t.Id.Equals(todo.Id));
                        if(todoModel != null)
                        {
                            todoModel.Title = todo.Title; 
                            todoModel.Content = todo.Content; 
                            todoModel.Status = todo.Status;
                        }
                    }
                }
                else //新增
                {
                    var todoResult =  await toDoService.AddAsync(todo);
                    if (todoResult.Status)
                    {
                        ToDoDtos.Add((ToDoDto)todoResult.Result);
                    }
                }
            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        async void AddMemo(MemoDto model)
        {
            DialogParameters parameters = new DialogParameters();

            if (model != null)
            {
                parameters.Add("value", model);
            }

            var dialogResult = await dialog.ShowDialog("AddMemoView", parameters);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var memo = dialogResult.Parameters.GetValue<MemoDto>("value");

                if (memo.Id > 0)
                {
                    var updateResult = await memoService.UpdateAsync(memo);

                    if (updateResult.Status)
                    {
                        var memoModel = ToDoDtos.FirstOrDefault(t => t.Id.Equals(memo.Id));
                        if (memoModel != null)
                        {
                            memoModel.Title = memo.Title;
                            memoModel.Content = memo.Content;
                        }
                    }
                }
                else //新增
                {
                    var memoResult = await memoService.AddAsync(memo);
                    if (memoResult.Status)
                    {
                        MemoDtos.Add((MemoDto)memoResult.Result);
                    }
                }
            }
        }

        private async void Complete(ToDoDto obj)
        {
            var todoResult =  await toDoService.UpdateAsync(obj);
            if (todoResult.Status)
            {
                var todoModel = ToDoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if(todoModel != null)
                {
                    ToDoDtos.Remove(todoModel);
                }
            }
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

            
        }
    }
}
