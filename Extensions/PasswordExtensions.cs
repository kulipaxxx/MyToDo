using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Extensions
{
    public class PasswordExtensions
    {


        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtensions), new PropertyMetadata(string.Empty,OnPasswordPropertyChanged));

        static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passWord = sender as PasswordBox;

            string password = (string)e.NewValue;

            if(passWord != null && passWord.Password != password) {
                passWord.Password = password;
            }
        }

    }

    public class PasswordBehavior: Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            //注册事件
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox =  sender as PasswordBox;
            string password = PasswordExtensions.GetPassword(passwordBox);

            if(password != null && passwordBox.Password != password)
            {
                PasswordExtensions.SetPassword(passwordBox, password);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            //取消事件
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}
