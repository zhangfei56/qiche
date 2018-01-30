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
using MaterialDesignThemes.Wpf;
using System.Globalization;

namespace QicheClient.pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AddVehiclePage : Page
    {
        public AddVehiclePage()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Vehicle v = new Vehicle();
            vehicleModel.DataContext = v;

            this.vehicleModel.DataContext = v;
        }

        private void btnPolicyTypeDelete_Click(object sender, RoutedEventArgs e)
        {
            policyTypeView.SelectedItem = ((PackIcon)sender).DataContext;

            //在数据集合中删除此元素 
            Vehicle vehicle = (Vehicle)this.vehicleModel.DataContext;
            vehicle.policy.PolicyTypes.RemoveAt(policyTypeView.SelectedIndex);
            policyTypeView.Items.Refresh();
        }

        private void btnPolicyTypeAdd_Click(object sender, RoutedEventArgs e)
        {
            PolicyType type = new PolicyType();
            Vehicle vehicle = (Vehicle)this.vehicleModel.DataContext;
            vehicle.policy.PolicyTypes.Add(type);
            policyTypeView.Items.Refresh();
        }

        private void btnVehicleAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
