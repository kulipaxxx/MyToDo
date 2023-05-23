
using MyToDo.API.Context;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
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
            , IDialogHostService dialog) : base(provider)
        {
            Title = $"你好, CodeC，{DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            CreateTaskBars();
            ExecuteCommand = new DelegateCommand<string>(Execute);

            this.toDoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.regionManager = provider.Resolve<IRegionManager>();
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            ToDoCompleteCommand = new DelegateCommand<ToDoDto>(Complete);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            this.dialog = dialog;
        }

        private void Navigate(TaskBar obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Target)) return;

            NavigationParameters parameters =  new NavigationParameters();
            
            if(obj.Title == "已完成")
            {
                parameters.Add("Value", 2);
            }

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, parameters);
        }



        #region 属性

        private readonly IToDoService toDoService;

        private readonly IMemoService memoService;

        private readonly IRegionManager regionManager;

        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }
        public DelegateCommand<ToDoDto> ToDoCompleteCommand { get; private set; }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private readonly IDialogHostService dialog;

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }


        private SummaryDto summaryDto;

        public SummaryDto SummaryDto
        {
            get { return summaryDto; }
            set { summaryDto = value; RaisePropertyChanged(); }
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

            if (model != null)
            {
                parameters.Add("value", model);
            }

            var dialogResult = await dialog.ShowDialog("AddToDoView", parameters);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var todo = dialogResult.Parameters.GetValue<ToDoDto>("value");

                if (todo.Id > 0)
                {
                    var updateResult = await toDoService.UpdateAsync(todo);

                    if (updateResult.Status)
                    {
                        var todoModel = SummaryDto.ToDoList.FirstOrDefault(t => t.Id.Equals(todo.Id));
                        if (todoModel != null)
                        {
                            todoModel.Title = todo.Title;
                            todoModel.Content = todo.Content;
                            todoModel.Status = todo.Status;
                        }
                    }
                }
                else //新增
                {
                    var todoResult = await toDoService.AddAsync(todo);
                    if (todoResult.Status)
                    {
                        summaryDto.Sum += 1;
                        
                        SummaryDto.ToDoList.Add((ToDoDto)todoResult.Result);
                        summaryDto.CompletedRatio = (summaryDto.CompletedCount / (double)summaryDto.Sum).ToString("0%");
                        this.Refresh();
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
                        var memoModel = SummaryDto.MemoList.FirstOrDefault(t => t.Id.Equals(memo.Id));
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
                        summaryDto.MemoeCount += 1;
                        SummaryDto.MemoList.Add((MemoDto)memoResult.Result);
                        this.Refresh();
                    }
                }
            }
        }

        private async void Complete(ToDoDto obj)
        {
            var todoResult = await toDoService.UpdateAsync(obj);
            if (todoResult.Status)
            {
                var todoModel = SummaryDto.ToDoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if (todoModel != null)
                {
                    SummaryDto.ToDoList.Remove(todoModel);
                    summaryDto.CompletedCount += 1;
                    summaryDto.CompletedRatio = (summaryDto.CompletedCount / (double)summaryDto.Sum).ToString("0%");

                    this.Refresh();
                }
            }
        }
        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            taskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView" });
            taskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView" });
            taskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "" });
            taskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView" });
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var summaryResult = await toDoService.GetSummaryAsync();

            if (summaryResult.Status)
            {
                SummaryDto = summaryResult.Result;
                Refresh();
            }

            base.OnNavigatedTo(navigationContext);
        }

        void Refresh()
        {
            TaskBars[0].Content = SummaryDto.Sum.ToString();
            TaskBars[1].Content = SummaryDto.CompletedCount.ToString();
            TaskBars[2].Content = SummaryDto.CompletedRatio;
            TaskBars[3].Content = SummaryDto.MemoeCount.ToString();
        }
    }
}
