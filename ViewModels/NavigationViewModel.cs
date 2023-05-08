using Microsoft.EntityFrameworkCore.Metadata;
using MyToDo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase,INavigationAware
    {
        private readonly IContainerProvider provider;
        public readonly IEventAggregator eventAggregator;

        public NavigationViewModel(IContainerProvider provider)
        {
            this.provider = provider;
            eventAggregator = provider.Resolve<IEventAggregator>();
        }

        //虚函数， virtual 可以实现可以不实现
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public void UpdateLoading(bool isOpen)
        {
            eventAggregator.UpdateLoading(new Common.Event.UpdateModel() { isOpen = isOpen });
        }
    }
}
