using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Net.NetworkInformation;
using Windows.Networking.Connectivity;

namespace KontrolkaAlarmu
{
    public partial class MainPage : PhoneApplicationPage
    {
        public RpiWebService wsTemp;
        public RpiWebService wsHum;
        public RpiWebService wsMove;
        public string humSinceDate;
        public string tempSinceDate;
        public string moveSinceDate;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            alarmListBox.Items.Clear();
            chartTemperature.BorderThickness = new Thickness(0);
            if(!CheckNetworkConnection())
                MessageBox.Show("Brak połączenia z Internetem");
            else
                GetAndSetData();
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        public static bool CheckNetworkConnection()
        {
            bool IsConnected = false;
            IsConnected = NetworkInterface.GetIsNetworkAvailable();
            if (IsConnected)
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkConnectivityLevel connection = InternetConnectionProfile.GetNetworkConnectivityLevel();
                if (connection == NetworkConnectivityLevel.None || connection == NetworkConnectivityLevel.LocalAccess)
                {
                    IsConnected = false;
                }
            }
            return IsConnected;
        }

        public void GetAndSetData()
        {
            try
            {

                ReadSettings();
                RpiWebService wsTemp = new RpiWebService(chartTemperature, txtTempDate, txtTempValue, txtTempMinDate, txtTempMaxDate);
                wsTemp.GetData("temperature", tempSinceDate);
                RpiWebService wsHum = new RpiWebService(chartHumidity, txthumDate, txthumValue, txthumMinDate, txthumMaxDate);
                wsHum.GetData("humidity", humSinceDate);
                RpiWebService wsMove = new RpiWebService(alarmListBox);
                wsMove.GetData("move_detection", moveSinceDate);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void ShowMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public void ReadSettings()
        {
            // txtDisplay is a TextBlock defined in XAML.
            if (IsolatedStorageSettings.ApplicationSettings.Contains("dateHum"))
            {
                humSinceDate = IsolatedStorageSettings.ApplicationSettings["dateHum"].ToString();
            }
            else
            {
                humSinceDate = "2014-12-24";
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains("dateTemp"))
            {
                tempSinceDate = IsolatedStorageSettings.ApplicationSettings["dateTemp"].ToString();
            }
            else
            {
                tempSinceDate = "2014-12-24";
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains("dateMove"))
            {
                moveSinceDate = IsolatedStorageSettings.ApplicationSettings["dateMove"].ToString();
            }
            else
            {
                moveSinceDate = "2014-12-24";
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshEvent(object sender, EventArgs e)
        {
            if (!CheckNetworkConnection())
                MessageBox.Show("Brak połączenia z Internetem");
            else
                GetAndSetData();
        }

        private void ShowSettingsPageEvent(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

    }
    public class LineChart
    {
        public string Cat1 { get; set; }
        public double Line1 { get; set; }
    }
    public class LineDataCollection : List<LineChart> { }

  
}