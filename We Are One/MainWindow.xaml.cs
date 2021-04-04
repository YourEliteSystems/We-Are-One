#region Usings
using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using Newtonsoft.Json;
#endregion


namespace We_Are_One
{

    public partial class MainWindow : Window
    {
    	#region Functions
        private readonly List<string> urlList = new List<string>();
        private IList<RItem> stream { get; set; }
        private bool Enabled = false;
        private Boolean IsPlaying;
        #endregion

        public readonly string file = "stream.cfg";

        public MainWindow()
        {
            InitializeComponent();
            SenderListeLaden();
            initVLC();
            //XML_SenderlisteLaden();
            btnStop.IsEnabled = false;
            IsPlaying = false;
            lblBassVersion.Content = "";
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
        
        private void initVLC()
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;

            var libDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            this.VlcControl.SourceProvider.CreatePlayer(libDirectory);
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (senderView.SelectedIndex == 0)
            {
                RItem item = (RItem)senderView.SelectedItem;
                VlcControl.SourceProvider.MediaPlayer.Play(new Uri(item.Url));
                //MessageBox.Show(item.Url.Length.ToString());
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
                    VlcControl.SourceProvider.MediaPlayer.Stop();
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
        public void loadJson()
        {
            StreamReader r = new StreamReader("streams.json");
            string json = r.ReadToEnd();
            List<RItem> ritems = JsonConvert.DeserializeObject<List<RItem>>(json);
        }
        private void SlideVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //bp.Volume = (float)(slideVol.Value * 0.01f);
            VlcControl.SourceProvider.MediaPlayer.Audio.Volume = (int)slideVol.Value;
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
