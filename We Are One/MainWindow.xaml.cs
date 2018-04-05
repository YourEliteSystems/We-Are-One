#region Usings
using System;
using System.Collections.Generic;
using System.Windows;
using Un4seen.Bass;
using System.IO;
using System.Xml.Linq;
#endregion


namespace We_Are_One
{

    public partial class MainWindow : Window
    {
    	#region Functions
    	private readonly Bass_Net bp = new Bass_Net();
        private readonly List<string> urlList = new List<string>();
        private bool Enabled = false;
        private Boolean IsPlaying;
        private readonly XDocument reader = XDocument.Load("streams.xml");
        #endregion

        public readonly string file = "stream.cfg";

        public MainWindow()
        {
            BassNet.Registration("ramonoltmann@outlook.de", "2X373142324823");
            InitializeComponent();
            SenderListeLaden();
            //XML_SenderlisteLaden();
            btnStop.IsEnabled = false;
            IsPlaying = false;
            lblBassVersion.Content = Convert.ToString(Bass.BASSVERSION);
            lblStation.Content = String.Format("Station: None");
            if (Properties.Settings.Default.LautStearkeSpeichern)
            {
                slideVol.Value = Properties.Settings.Default.LautstrearkeWert;
            }
            else
            {
                slideVol.Value = 15;
            }
        }
        
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (senderView.SelectedIndex == 0)
            {
                RItem item = (RItem)senderView.SelectedItem;
				bp.PlayUrl(item.Url);
                lblStation.Content = String.Format("Station: {0}", item.Name.ToString());
                Enabled = true;
                btnStop.IsEnabled = true;
                IsPlaying = true;
            }else if (senderView.SelectedIndex < 0){
                MessageBox.Show("No Item Found");
            }
        }

        private void BtnStop_Click(object sender,RoutedEventArgs e)
        {
            if (Enabled == true) {
                if (IsPlaying == true)
                {
                    bp.Stop();
                    lblStation.Content = String.Format("Station: None");
                    btnStop.IsEnabled = false;
                }
            }
        }

        public void SenderListeLaden()
        {
            try
            {
                string[] array = GetStreamList();
                for(int i = 0;i<array.Length - 1; i += 2)
                {
                    urlList.Add(array[i + 1]);
                    RItem item = new RItem
                    {
                        Name = array[i],
                        Url = array[i + 1]
                    };
                    senderView.Items.Add(item);
                }
            }catch(IndexOutOfRangeException e)
            {
                MessageBox.Show(String.Format("Sie haben ein Fehler: {0}", e.ToString()));
            }catch(Exception e)
            {
                MessageBox.Show(String.Format("Sie haben ein Fehler: {0}", e.ToString()));
            }
        }
        /*public void XML_SenderlisteLaden()
        {
            RItem item = new RItem();
            //string[] array;
            //reader.ReadStartElement("stream");
            while (reader.)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name.ToString())
                    {
                        case "Station":
                            //item.Name = reader.ReadElementContentAsString();
                            item.Name = reader.ReadElementContentAsString();
                            break;
                        case "Url":
                            item.Url = reader.ReadElementContentAsString();
                            break;
                    }
                }
            }
            senderView.Items.Add(item);
        }*/
        private void SlideVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
			bp.Volume = (float)(slideVol.Value * 0.01f);
            Properties.Settings.Default.LautstrearkeWert = Convert.ToInt32(slideVol.Value);
            Properties.Settings.Default.Save();

        }

        private void Mt_About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void Mt_Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void Mt_Close_Click(object sender, RoutedEventArgs e){
        
        }

        public string[] GetStreamList()
        {
            string text = string.Empty;
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(file))
            {
                while ((str = streamReader.ReadLine()) != null)
                {
                    text += str;
                }
            }
            return text.Split(new char[]
            {
                ';'
            });
        }

    }
    
    public class RItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

}
