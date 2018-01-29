using ClientsLibrary;
using CommonLibrary;
using CommonLibrary.Model;
using HslCommunication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QicheClient.pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserListPage : Page
    {
        public UserListPage()
        {
            InitializeComponent();
        }

        CollectionViewSource view = new CollectionViewSource();
        ObservableCollection<UserAccount> customers = new ObservableCollection<UserAccount>();
        int currentPageIndex = 0;
        int itemPerPage = 20;
        int totalPage = 0;

        private void ShowCurrentPageIndex()
        {
            //this.tbCurrentPage.Text = (currentPageIndex + 1).ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.AccountUserList);
            List<UserAccount> s = null;
            if (result.IsSuccess)
            {
                JArray jarray = JArray.Parse(result.Content);
                s = JsonConvert.DeserializeObject<List<UserAccount>>(result.Content);
            }

            int itemcount = 107;
            for (int j = 0; j < s.Count; j++)
            {
                customers.Add(new UserAccount()
                {
                    //ID = j,
                    UserName = s[j].UserName,
                });
            }

            // Calculate the total pages
            totalPage = itemcount / itemPerPage;
            if (itemcount % itemPerPage != 0)
            {
                totalPage += 1;
            }

            view.Source = customers;

           // view.Filter += new FilterEventHandler(view_Filter);

            this.userListView.DataContext = view;
            ShowCurrentPageIndex();
           // this.tbTotalPage.Text = totalPage.ToString();
        }

        void view_Filter(object sender, FilterEventArgs e)
        {
            int index = customers.IndexOf((UserAccount)e.Item);

            if (index >= itemPerPage * currentPageIndex && index < itemPerPage * (currentPageIndex + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            // Display the first page
            if (currentPageIndex != 0)
            {
                currentPageIndex = 0;
                view.View.Refresh();
            }
            ShowCurrentPageIndex();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            // Display previous page
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                view.View.Refresh();
            }
            ShowCurrentPageIndex();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Display next page
            if (currentPageIndex < totalPage - 1)
            {
                currentPageIndex++;
                view.View.Refresh();
            }
            ShowCurrentPageIndex();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            // Display the last page
            if (currentPageIndex != totalPage - 1)
            {
                currentPageIndex = totalPage - 1;
                view.View.Refresh();
            }
            ShowCurrentPageIndex();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
