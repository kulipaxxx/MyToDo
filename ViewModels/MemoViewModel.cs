using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using MyToDo.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Regions;

namespace MyToDo.ViewModels
{
    public class MemoViewModel : NavigationViewModel
    {
        public MemoViewModel(IMemoService service, IContainerProvider provider) : base(provider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);

            this.service = service;

        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }

        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
        private ObservableCollection<MemoDto> memoDtos;
        private readonly IMemoService service;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }//通知更新 
        }


        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private string search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 下拉列表选中值
        /// </summary>
        private int selectIndex;

        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }

        private MemoDto currentDto;
        /// <summary>
        /// 编辑选中/新增时对象
        /// </summary>
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        public void Execute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": GetDataAsync(); break;
                case "保存": Save(); break;
            }
        }

        private void Add()
        {
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }

        private async void Selected(MemoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await service.GetFirstOfDefaultAsync(obj.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {
            }
            finally { UpdateLoading(false); }
        }

        private async void Delete(MemoDto obj)
        {
            var deleteResult = await service.DeleteAsync(obj.Id);
            if (deleteResult.Status)
            {
                var todos = MemoDtos.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if (todos != null)
                {
                    MemoDtos.Remove(todos);
                }
            }
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) ||
                string.IsNullOrWhiteSpace(CurrentDto.Content))
                return;

            try
            {
                UpdateLoading(true);

                //修改
                if (CurrentDto.Id > 0)
                {
                    var todoResult = await service.UpdateAsync(CurrentDto);
                    if (todoResult.Status)
                    {
                        var todo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                        }
                    }
                }
                else //新增
                {
                    var todoResult = await service.AddAsync(CurrentDto);
                    if (todoResult.Status)
                    {
                        MemoDtos.Add(todoResult.Result);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsRightDrawerOpen = false;
                UpdateLoading(false);
            }
        }



        async void GetDataAsync()
        {
            //开启动画
            UpdateLoading(true);

            var memos = await service.GetAllAsync(new Shared.Parameters.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Search = Search
            });
            if (memos.Status)
            {
                MemoDtos.Clear();
                foreach (var item in memos.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }

            UpdateLoading(false);
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            GetDataAsync();
        }

    }
}
