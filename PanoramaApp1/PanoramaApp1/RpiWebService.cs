using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace PanoramaApp1
{
    public class RpiWebService
    {

        public string xmlSource = @"http://mikipi.dlinkddns.com/api.php";
        public Humiditys humiditys;
        public Temperatures temperatures;
        public MoveDetections moveDetections;
        public AmCharts.Windows.QuickCharts.SerialChart chart;
        public ObservableCollection<LineChart> _data;
        public TextBlock dateBlock;
        public TextBlock valueBlock;
        public TextBlock txtMinDate;
        public TextBlock txtMaxDate;
        public ListBox listBox;
        WebClient downloader;
        private string webServiceSource = @"http://rpi.bitnamiapp.com/api.php";

        public class NoDataException : Exception 
        {
            public NoDataException(string type) : base(string.Format("Błąd danych typu:\r\n{0}", type)) { }

            public NoDataException() : base() { }
        }

        public RpiWebService(AmCharts.Windows.QuickCharts.SerialChart c, TextBlock d, TextBlock v,
             TextBlock min, TextBlock max)
        {
            _data = new ObservableCollection<LineChart>();
            chart = c;
            dateBlock = d;
            valueBlock = v;
            txtMaxDate = max;
            txtMinDate = min;
        }

        public RpiWebService(ListBox lb)
        {
            this.listBox = lb;
        }

        public RpiWebService() { }

        public void GetData(string table, string minDate = "", string maxDate = "")
        {
            xmlSource = webServiceSource + @"?table=" + table;
            if (minDate != "")
                xmlSource += "&min=" + minDate;
            if (maxDate != "")
                xmlSource += "&max=" + maxDate;
            downloader = new WebClient();
            downloader.UseDefaultCredentials = true;
            Uri uri = new Uri(xmlSource, UriKind.Absolute);
            switch (table)
            {
                case "temperature":
                    downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(TemperatureDataDownload);
                    break;
                case "humidity":
                    downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HumidityDataDownload);
                    break;
                case "move_detection":
                    downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(MoveDataDownload);

                    break;
            }
            downloader.DownloadStringAsync(uri);
            // ProgBar.IsIndeterminate = true;
            // ProgressDownload.Text = "Please by patient" + Environment.NewLine + "download data is in progress";
        }

        private void TemperatureDataDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || e.Error != null)
                {
                    throw e.Error;
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Temperatures));
                    XDocument document = XDocument.Parse(e.Result);
                    temperatures = (Temperatures)serializer.Deserialize(document.CreateReader());
                    if (temperatures.TEMPERATURE == null)
                        throw new NoDataException("Temperatura");
                    Temperature t;
                    LineChart lc = new LineChart();
                    for (int i = 0; i < temperatures.TEMPERATURE.Length; i++)
                    {
                        t = temperatures.TEMPERATURE[i];
                        lc = new LineChart();

                        if (i == 0)
                            txtMinDate.Text = "Od: " + string.Format("{0} {1}", Convert.ToDateTime(t.DATE).ToShortDateString(), Convert.ToDateTime(t.DATE).ToShortTimeString());
                        if (i == temperatures.TEMPERATURE.Length - 1)
                            txtMaxDate.Text = "Do: " + string.Format("{0} {1}", Convert.ToDateTime(t.DATE).ToShortDateString(), Convert.ToDateTime(t.DATE).ToShortTimeString());

                        lc.Line1 = Convert.ToDouble(t.VALUE);
                        lc.Cat1 = "";
                        _data.Add(lc);
                        if (i == temperatures.TEMPERATURE.Length - 1)
                        {
                            dateBlock.Text = t.DATE;
                            valueBlock.Text = string.Format("{0} {1}", t.VALUE, t.UNIT);
                        }
                    }
                    chart.DataSource = _data;

                }
            }
            catch (NoDataException de)
            {
                MainPage.ShowMessageBox(de.Message);
            }
            catch (Exception ex)
            {
                if(ex.InnerException!=null)
                    MainPage.ShowMessageBox(ex.InnerException.Message);
                else
                    MainPage.ShowMessageBox(ex.Message);
            }

        }

        private void HumidityDataDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null || e.Error != null)
                {
                    throw e.Error;
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Humiditys));
                    XDocument document = XDocument.Parse(e.Result);
                    humiditys = (Humiditys)serializer.Deserialize(document.CreateReader());
                    if (humiditys.HUMIDITY == null)
                        throw new NoDataException("Wilgotność");
                    _data = new ObservableCollection<LineChart>();
                    Humidity h;
                    LineChart lc = new LineChart();
                    for(int i =0;i<humiditys.HUMIDITY.Length;i++)
                    {
                        h = humiditys.HUMIDITY[i];
                        lc = new LineChart();

                        if (i == 0)
                            txtMinDate.Text = "Od: " + string.Format("{0} {1}", Convert.ToDateTime(h.DATE).ToShortDateString(), Convert.ToDateTime(h.DATE).ToShortTimeString());
                        if (i == humiditys.HUMIDITY.Length - 1)
                            txtMaxDate.Text = "Do: " + string.Format("{0} {1}", Convert.ToDateTime(h.DATE).ToShortDateString(), Convert.ToDateTime(h.DATE).ToShortTimeString());

                        lc.Line1 = Convert.ToDouble(h.VALUE);
                        lc.Cat1 = "";
                        _data.Add(lc);

                        if(i==humiditys.HUMIDITY.Length-1)
                        {
                            dateBlock.Text = h.DATE;
                            valueBlock.Text = string.Format("{0} {1}", h.VALUE, h.UNIT);
                        }
                    }
                    chart.DataSource = _data;
                    
                }
            }
            catch (NoDataException de)
            {
                MainPage.ShowMessageBox(de.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MainPage.ShowMessageBox(ex.InnerException.Message);
                else
                    MainPage.ShowMessageBox(ex.Message);
            }

        }      

        private void MoveDataDownload(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                    if (e.Result == null || e.Error != null)
                    {
                        throw e.Error;
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(MoveDetections));
                        XDocument document = XDocument.Parse(e.Result);
                        moveDetections = (MoveDetections)serializer.Deserialize(document.CreateReader());
                        if (moveDetections.MOVE_DETECTION == null)
                            throw new NoDataException("Wykryty Ruch");
                        MoveDetection m;
                        for (int i = moveDetections.MOVE_DETECTION.Length - 1; i > -1; i--)
                        {
                            m = moveDetections.MOVE_DETECTION[i];

                            listBox.Items.Add(m.DATE);
                        }

                    }
                
            }
            catch (NoDataException de)
            {
                MainPage.ShowMessageBox(de.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MainPage.ShowMessageBox(ex.InnerException.Message);
                else
                    MainPage.ShowMessageBox(ex.Message);
            }


        }

    }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute("humiditys",Namespace = "", IsNullable = false)]
        public partial class Humiditys
        {

            private Humidity[] hUMIDITYField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("humidity")]
            public Humidity[] HUMIDITY
            {
                get
                {
                    return this.hUMIDITYField;
                }
                set
                {
                    this.hUMIDITYField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class Humidity
        {

            private byte idField;

            private string dATEField;

            private decimal vALUEField;

            private string uNITField;

            /// <remarks/>
            public byte ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string DATE
            {
                get
                {
                    return this.dATEField;
                }
                set
                {
                    this.dATEField = value;
                }
            }

            /// <remarks/>
            public decimal VALUE
            {
                get
                {
                    return this.vALUEField;
                }
                set
                {
                    this.vALUEField = value;
                }
            }

            /// <remarks/>
            public string UNIT
            {
                get
                {
                    return this.uNITField;
                }
                set
                {
                    this.uNITField = value;
                }
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute("temperatures",Namespace = "", IsNullable = false)]
        public partial class Temperatures
        {

            private Temperature[] tEMPERATUREField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("temperature")]
            public Temperature[] TEMPERATURE
            {
                get
                {
                    return this.tEMPERATUREField;
                }
                set
                {
                    this.tEMPERATUREField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class Temperature
        {

            private byte idField;

            private string dATEField;

            private decimal vALUEField;

            private string uNITField;

            /// <remarks/>
            public byte ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string DATE
            {
                get
                {
                    return this.dATEField;
                }
                set
                {
                    this.dATEField = value;
                }
            }

            /// <remarks/>
            public decimal VALUE
            {
                get
                {
                    return this.vALUEField;
                }
                set
                {
                    this.vALUEField = value;
                }
            }

            /// <remarks/>
            public string UNIT
            {
                get
                {
                    return this.uNITField;
                }
                set
                {
                    this.uNITField = value;
                }
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute("move_detections",Namespace = "", IsNullable = false)]
        public partial class MoveDetections
        {

            private MoveDetection[] mOVE_DETECTIONField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("move_detection")]
            public MoveDetection[] MOVE_DETECTION
            {
                get
                {
                    return this.mOVE_DETECTIONField;
                }
                set
                {
                    this.mOVE_DETECTIONField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MoveDetection
        {

            private byte idField;

            private string dATEField;

            /// <remarks/>
            public byte ID
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public string DATE
            {
                get
                {
                    return this.dATEField;
                }
                set
                {
                    this.dATEField = value;
                }
            }
        }

        public static partial class MyExtensions
        {
            public static Task<string> DownloadAsync(this WebClient wc, string url)
            {
                TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
                DownloadStringCompletedEventHandler completed = null;

                completed = (s, e) =>
                {
                    try
                    {
                        tcs.SetResult(e.Result);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex.InnerException ?? ex);
                    }
                    finally
                    {
                        wc.DownloadStringCompleted -= completed;
                    }
                };

                wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
                wc.DownloadStringCompleted += completed;
                wc.DownloadStringAsync(new Uri(url));

                return tcs.Task;
            }
        }



}
