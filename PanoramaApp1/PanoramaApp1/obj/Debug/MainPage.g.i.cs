﻿#pragma checksum "E:\Dokumenty\Projekty Programowanie\PanoramaApp1\PanoramaApp1\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "83FFC5223D316F3B4B7EB7E98E1D57BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AmCharts.Windows.QuickCharts;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PanoramaApp1 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal AmCharts.Windows.QuickCharts.SerialChart chartTemperature;
        
        internal System.Windows.Controls.StackPanel tempIntervalPanel1;
        
        internal System.Windows.Controls.TextBlock txtTempMinDate;
        
        internal System.Windows.Controls.StackPanel tempIntervalPanel2;
        
        internal System.Windows.Controls.TextBlock txtTempMaxDate;
        
        internal System.Windows.Controls.StackPanel tempDatePanel;
        
        internal System.Windows.Controls.TextBlock txtTempDate;
        
        internal System.Windows.Controls.StackPanel tempValuePanel;
        
        internal System.Windows.Controls.TextBlock txtTempValue;
        
        internal AmCharts.Windows.QuickCharts.SerialChart chartHumidity;
        
        internal System.Windows.Controls.StackPanel humIntervalPanel1;
        
        internal System.Windows.Controls.TextBlock txthumMinDate;
        
        internal System.Windows.Controls.StackPanel humIntervalPanel2;
        
        internal System.Windows.Controls.TextBlock txthumMaxDate;
        
        internal System.Windows.Controls.StackPanel humDatePanel;
        
        internal System.Windows.Controls.TextBlock txthumDate;
        
        internal System.Windows.Controls.StackPanel humValuePanel;
        
        internal System.Windows.Controls.TextBlock txthumValue;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox alarmListBox;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PanoramaApp1;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.chartTemperature = ((AmCharts.Windows.QuickCharts.SerialChart)(this.FindName("chartTemperature")));
            this.tempIntervalPanel1 = ((System.Windows.Controls.StackPanel)(this.FindName("tempIntervalPanel1")));
            this.txtTempMinDate = ((System.Windows.Controls.TextBlock)(this.FindName("txtTempMinDate")));
            this.tempIntervalPanel2 = ((System.Windows.Controls.StackPanel)(this.FindName("tempIntervalPanel2")));
            this.txtTempMaxDate = ((System.Windows.Controls.TextBlock)(this.FindName("txtTempMaxDate")));
            this.tempDatePanel = ((System.Windows.Controls.StackPanel)(this.FindName("tempDatePanel")));
            this.txtTempDate = ((System.Windows.Controls.TextBlock)(this.FindName("txtTempDate")));
            this.tempValuePanel = ((System.Windows.Controls.StackPanel)(this.FindName("tempValuePanel")));
            this.txtTempValue = ((System.Windows.Controls.TextBlock)(this.FindName("txtTempValue")));
            this.chartHumidity = ((AmCharts.Windows.QuickCharts.SerialChart)(this.FindName("chartHumidity")));
            this.humIntervalPanel1 = ((System.Windows.Controls.StackPanel)(this.FindName("humIntervalPanel1")));
            this.txthumMinDate = ((System.Windows.Controls.TextBlock)(this.FindName("txthumMinDate")));
            this.humIntervalPanel2 = ((System.Windows.Controls.StackPanel)(this.FindName("humIntervalPanel2")));
            this.txthumMaxDate = ((System.Windows.Controls.TextBlock)(this.FindName("txthumMaxDate")));
            this.humDatePanel = ((System.Windows.Controls.StackPanel)(this.FindName("humDatePanel")));
            this.txthumDate = ((System.Windows.Controls.TextBlock)(this.FindName("txthumDate")));
            this.humValuePanel = ((System.Windows.Controls.StackPanel)(this.FindName("humValuePanel")));
            this.txthumValue = ((System.Windows.Controls.TextBlock)(this.FindName("txthumValue")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.alarmListBox = ((System.Windows.Controls.ListBox)(this.FindName("alarmListBox")));
        }
    }
}
