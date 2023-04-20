using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    ///  系统导航菜单   
    /// </summary>
    public class MenuBar : BindableBase
    {
        public string? icon;
        
        public string Icon { get { return icon; } set { icon = value; } }

        /// <summary>
        /// 菜单名称
        /// </summary>
        private string? title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string? nameSpace;

        /// <summary>
        /// 菜单命名空间
        /// </summary>
        public string? NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }

    }

    


}
