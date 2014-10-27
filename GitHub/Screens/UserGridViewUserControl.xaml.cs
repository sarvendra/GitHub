using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Model;
using GitHub.Utility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GitHub.Screens
{
    public partial class UserGridViewUserControl : UserControl
    {
        public UserGridViewUserControl()
        {
            InitializeComponent();
        }

        private void UserLongListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            User selectedUser = selector.SelectedItem as User;
            if (selectedUser == null)
                return;
            string uri = PageLocator.USER_PAGE + "?name=" + selectedUser.login;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}
