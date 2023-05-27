﻿using MyToDo.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel()
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }



        public string Title { get; private set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        #region 属性
        public DelegateCommand<string> ExecuteCommand;

        private string account;

        public string Account
        {
            get { return account; }
            //实现通知
            set { account = value; RaisePropertyChanged(); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }





        #endregion

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
            }
        }

        void Login()
        {

        }

        void LoginOut()
        {

        }

    }
}
