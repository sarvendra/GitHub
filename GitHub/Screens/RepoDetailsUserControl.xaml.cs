using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using GitHub.Model;
using GitHub.Utility;
using GitHub.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GitHub.Screens
{
    public partial class RepoDetailsUserControl : UserControl
    {
        public event EventHandler branchListPickerMouseEnterEvent;
        public event EventHandler OwnerButtonClickEvent;

        public RepoDetailsUserControl()
        {
            InitializeComponent();
        }

        private void BranchListPicker_OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            branchListPickerMouseEnterEvent(sender, e);
        }

        private void ownerButton_Click(object sender, RoutedEventArgs e)
        {
            OwnerButtonClickEvent(sender, e);
        }
    }
}
