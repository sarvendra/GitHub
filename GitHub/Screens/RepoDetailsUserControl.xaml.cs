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
    public partial class RepoDetailsUserControl : UserControl
    {
        public event  EventHandler branchListPickerSelectionChangedEvent;
        public event EventHandler branchListPickerMouseEnterEvent;
        public event EventHandler OwnerButtonClickEvent;
        public event EventHandler DownloadButtonClickEvent;

        public RepoDetailsUserControl()
        {
            InitializeComponent();
        }

        private void branchListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            branchListPickerSelectionChangedEvent(sender, e);
        }

        private void branchListPicker_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            branchListPickerMouseEnterEvent(sender, e);
        }

        private void ownerButton_Click(object sender, RoutedEventArgs e)
        {
            OwnerButtonClickEvent(sender, e);
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadButtonClickEvent(sender, e);
        }
    }
}
