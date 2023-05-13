using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel(IRegionManager regionManager)
        {
            menuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                {
                    journal.GoBack();
                }
            });

            GoForwardCommand = new DelegateCommand(() => {
                if (journal != null && journal.CanGoForward)
                {
                    journal.GoForward();
                }
            });
        }

        private void Navigate(MenuBar obj)
        {
            if(obj == null || string.IsNullOrEmpty(obj.NameSpace)) {
                return;
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back => { 
                journal = back.Context.NavigationService.Journal;
            });

            
        }

        //委派命令
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        //上一步跳转
        public DelegateCommand GoBackCommand { get; private set; }
        //跳转下一步
        public DelegateCommand GoForwardCommand { get; private set; }

        private ObservableCollection<MenuBar> menuBars;
        //注册事件
        private readonly IRegionManager regionManager;
        //事件日志上一步下一步
        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "代办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingView" });
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
