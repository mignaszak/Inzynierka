using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PanoramaApp1
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            ReadSettings();
        }

        public void SaveSettingsEvent(object sender, EventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            // txtInput is a TextBox defined in XAML.
            if (!settings.Contains("dateHum"))
            {
                settings.Add("dateHum", dateHum.Value.ToString());
            }
            else
            {
                settings["dateHum"] = dateHum.Value.ToString();
            }

            if (!settings.Contains("dateTemp"))
            {
                settings.Add("dateTemp", dateTemp.Value.ToString());
            }
            else
            {
                settings["dateTemp"] = dateTemp.Value.ToString();
            }

            if (!settings.Contains("dateMove"))
            {
                settings.Add("dateMove", dateMove.Value.ToString());
            }
            else
            {
                settings["dateMove"] = dateMove.Value.ToString();
            }
            settings.Save();
            NavigationService.GoBack();
        }

        public void ReadSettings()
        {
            // txtDisplay is a TextBlock defined in XAML.
            if (IsolatedStorageSettings.ApplicationSettings.Contains("dateHum"))
            {
                dateHum.Value = Convert.ToDateTime(IsolatedStorageSettings.ApplicationSettings["dateHum"]);
            }
            else
            {
                dateHum.Value = new DateTime(2014, 12, 24);
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains("dateTemp"))
            {
                dateTemp.Value = Convert.ToDateTime(IsolatedStorageSettings.ApplicationSettings["dateTemp"]);
            }
            else
            {
                dateTemp.Value = new DateTime(2014, 12, 24);
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains("dateMove"))
            {
                dateMove.Value = Convert.ToDateTime(IsolatedStorageSettings.ApplicationSettings["dateMove"]);
            }
            else
            {
                dateMove.Value = new DateTime(2014, 12, 24);
            }
        }

        private void BackEvent(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void dateTemp_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime > DateTime.Now)
            {
                try
                {
                    MessageBox.Show("Data nie może być przyszła\r\nNadaj nową wartość");
                    dateTemp.Value = e.OldDateTime; 
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void dateHum_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            if (e.NewDateTime > DateTime.Now)
            {
                try
                {
                    MessageBox.Show("Data nie może być przyszła\r\nNadaj nową wartość");
                    dateHum.Value = e.OldDateTime;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void dateMove_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

            if (e.NewDateTime > DateTime.Now)
            {
                try
                {
                    MessageBox.Show("Data nie może być przyszła\r\nNadaj nową wartość");
                    dateMove.Value = e.OldDateTime;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }
    }
}