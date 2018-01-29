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
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QicheClient.pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class VehicleListPage : Page
    {
        public VehicleListPage()
        {
            InitializeComponent();
        }

        CollectionViewSource view = new CollectionViewSource();
        ObservableCollection<Vehicle> customers = new ObservableCollection<Vehicle>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.VehicleList);
            List<Vehicle> s = null;
            if (result.IsSuccess)
            {
                s = JsonConvert.DeserializeObject<List<Vehicle>>(result.Content);
            }

            for (int j = 0; j < s.Count; j++)
            {
                customers.Add(new Vehicle()
                {
                    Id = s[j].Id,
                    VehicleNumber = s[j].VehicleNumber,
                    CompanyType = s[j].CompanyType,
                    policy = s[j].policy,
                    owner = s[j].owner,
                    VehicleType = s[j].VehicleType
                });
            }          

            view.Source = customers;

            this.vehicleListView.DataContext = view;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Vehicle vehicle = (Vehicle)this.vehicleListView.SelectedItem;

            string json = JsonConvert.SerializeObject(vehicle);
            OperateResult<string> result = UserClient.Net_simplify_client.ReadFromServer(CommonHeadCode.SimplifyHeadCode.VehicleUpdate, json);

            if (result.IsSuccess)
            {
                if (result.Content == SoftResources.SystemResouce.Success)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        //this.DialogResult = true;
                        return;
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        //SetInformationString("用户名或者密码错误");
                    }));


                }
            }
            else
            {
                //SetInformationString(result.Message);
            }
        }
    }
}
