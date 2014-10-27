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
    public partial class RepoListUserControl : UserControl
    {
        public RepoListUserControl()
        {
            InitializeComponent();
        }

        private void repoLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            Repo repo = selector.SelectedItem as Repo;
            if (repo == null)
                return;
            string uri = PageLocator.REPO_PAGE+"?reponame=" + repo.name + "&ownername=" + repo.owner.login;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
            selector.SelectedItem = null;
        }
    }
}
