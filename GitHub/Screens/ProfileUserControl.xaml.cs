using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GitHub.Model;

namespace GitHub.Screens
{
    public partial class ProfileUserControl : UserControl
    {
        public ProfileUserControl()
        {
            InitializeComponent();
        }

        public void SetUserInfo(User user)
        {
            string company = user.company;
            if (!string.IsNullOrEmpty(company))
            {
                this.company.Text = company;
                this.companyStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.companyStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            string location = user.location;
            if (!string.IsNullOrEmpty(location))
            {
                this.location.Text = location;
                this.locationStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.locationStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            string email = user.email;
            if (!string.IsNullOrEmpty(email))
            {
                this.email.Text = email;
                this.emailStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.emailStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            string date = user.created_at;
            if (!string.IsNullOrEmpty(date))
            {
                // Joining date
                this.dateStackPanel.Visibility = System.Windows.Visibility.Visible;
                this.date.Text = user.created_at.Split('T')[0];
            }
            else
            {
                this.dateStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

        }
    }
}
