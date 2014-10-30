using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GitHub.Deserializer;
using GitHub.Model;
using GitHub.Utility;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace GitHub.ViewModels
{
    public class DirectoryViewModel : ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

        private ObservableCollection<BranchContent> branchContentItems;

        public ObservableCollection<BranchContent> BranchContentItems
        {
            get
            {
                return branchContentItems;
            }

            set
            {
                branchContentItems = value;
                RaisePropertyChanged("BranchContentItems");
            }
        }

        private BranchContent selectedBranchContent;

        public BranchContent SelectedBranchContent
        {
            get
            {
                return selectedBranchContent;
            }

            set
            {
                selectedBranchContent = value;
                if (selectedBranchContent != null)
                {
                    // Do your logic
                    string uri = selectedBranchContent.url;
                    // navigate to direcotry explorer page
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(PageLocator.DIRECTORY_PAGE + "?uri=" + uri, UriKind.Relative));
                    selectedBranchContent = null;
                }
                RaisePropertyChanged("SelectedBranchContent");
            }
        }

        public DirectoryViewModel()
        {
            branchContentItems = new ObservableCollection<BranchContent>();
        }

        public async Task GetBranchContentsAsync(string branchUri)
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetAsyncStringResponse(branchUri);
            if (response == null)
                return;
            if (jsonDeserializer == null)
            {
                jsonDeserializer = new JSONDeserializer();
            }
            List<BranchContent> branches = jsonDeserializer.DeserializeBranchContents(response);
            if (branches == null)
                return;
            List<BranchContent> SortedList = branches.OrderBy(o => o.type).ToList();
            branchContentItems.Clear();
            foreach (var branch in SortedList)
            {
                branchContentItems.Add(branch);
            }
        }

        public bool IsLoggedIn()
        {
            manager = GitHubManager.Instance;
            return manager.IsLoggedIn();
        }
    }
}
