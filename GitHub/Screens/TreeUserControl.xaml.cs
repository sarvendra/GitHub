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
    public partial class TreeUserControl : UserControl
    {
        public TreeUserControl()
        {
            InitializeComponent();
        }

        private void treeLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            BranchContent branchcontent = selector.SelectedItem as BranchContent;
            if (branchcontent == null)
                return;
            if (branchcontent.type == "dir")
            {
                string uri = branchcontent.url;
                // navigate to direcotry explorer page
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(PageLocator.DIRECTORY_PAGE + "?uri=" + uri, UriKind.Relative));
            }
            selector.SelectedItem = null;
        }
    }
}
