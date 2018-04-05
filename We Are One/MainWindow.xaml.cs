#region Usings
using nUpdate.Updating;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using Un4seen.Bass;
#endregion


namespace We_Are_One
{

    public partial class MainWindow : Window
    {
    	#region Functions
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        UpdateManager manager = new UpdateManager(new Uri("http://status.yourelitesystems.de/updates/updates.json"), "<RSAKeyValue><Modulus>39zjS6xiay/zoHo43iVtaLRFYl7gKbClteNB4+1rNzfskGeCvDFxX7DstmHc7bi4dl9w8+3MTTx03MUaJLWSBx4WUtmiMgwlRisgzaU3GXEFyAysUsxXlARVNOLV+QeRkxrxx2m9QbEVnbnS5ysPzXy66jk3KcgaSMM/+bibR/5o4Ndgy+tc82MxMg9IWCnO9xzeVaLkxcU1v5VnJIYc3Ia+KSCf26Y8KT8Cws03dwczWZ+n1L0RwHYzkgWpj7aUJErnj0hnPihsLolkhAe0RkdDyGfbhdBwnF8CHoia+zjVWGfdHLoTk/Rrc8ENV8C1YZa5J8iieK0os7oGJqkTJrEI1iIcqvNyqlLyDBQms0f9XcNhAr9Zf5PAJrvgvkeBRcMh9oSPXEmTFbo5oxrg/3JaCpIKySF7aPBViUDru+t5qXFREzlMTIv7Hdkavndf8iDYESAt05G+CAiZ12y1CzWaT3TrWsyG3LFexlwklPjto8nz9N7mIc1GzC2FFGSfpyddiGXHIDRqsLC8cPhjQakEaXD2eu00+HkWGJu1LWF8guOuawJtRdKmx2kz1DTKYgaiilVMTpOwPSNCpLukKm/4cPbfdKyhh/QBaxg0uAMCceWRjnw4gn+s4BIDoNogcw20bPsMYzZKyKmoKjGMT6vkUYfA2JPOpdQofyvXZzNdkpqN5NRJJAKj7lhtyr2Ltjkc4qf38zMaSLFAw76kcydOOAixrRY3YcSF0fxrwM7/mLXIAMs4tfpqtGTJEMD1HNqsByOaPb5nJLbRXTeR0Vxb9vPwpiVnFxXKO/NUn3f4sM+nf2/W4Xx+54e3y0YIlOhGrok4QflgGiPtL46MKiFmOxyZgSxr3M26hMZE53q+g6UZ1sP66g+xRLBViqXkCiVrF/8yfornqFmQNqmECSZ1u+NSiorJKr8RMtrg6QwG/mOQdx/69Y0SVheTpHd9TF4EdX09bp3g0SGZSy/83Ycj2zrIz0n0XduYDv8XWlY5uoA/dmPSaBmQjh26qTUIbkndUCBAIB41EVEYbiUm8JXp1VtB2+hcsdwiX7uUhbpZZuv3pHCoCkozatgDqttwohKXORn96XzDrXH4wIzAEtYxh6wGcG9f6qfHoNojhREJnbNHe1p0cJf6W0DIfrTlVsIVECcsawO19qi6r38aIAxSEeF1vWreJPUX+SFvowgsvI/e5SJw+GhsxhyYI+rUFzen4moE5wFS+xRv/uXYOVSt+g+kodKKt5FJ2t+iNRyHbJeWQ7zqh5gDVZZZ6kh3dq9Z5VUSj0R1l0C2xjWLpWLVENhhnk8aJs9Lh9dwMpzzAOaE+IpWkh8FuZ9X4bEa1kuBfCyUT4o22/bLZxqrGQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new CultureInfo("en"));
        MusicPlayer mp = new MusicPlayer();
        StreamRW stream = new StreamRW("stream.cfg");
        private List<string> urlList = new List<string>();
        private Boolean IsPlaying;
        #endregion

        public MainWindow()
        {
            BassNet.Registration("ramonoltmann@outlook.de", "2X373142324823");
            InitializeComponent();
            senderListeLaden();
            mp.InitialBass();
            btnStop.IsEnabled = false;
            IsEnabled = false;
            IsPlaying = false;
            if (Properties.Settings.Default.LautStearkeSpeichern)
            {
                slideVol.Value = Properties.Settings.Default.LautstrearkeWert;
            }
            else
            {
                slideVol.Value = 15;
            }
            #region nUpdate
            var succededArgument = new UpdateArgument("success", UpdateArgumentExecutionOptions.OnlyOnSucceeded);
            var failedArgument = new UpdateArgument("fail", UpdateArgumentExecutionOptions.OnlyOnFaulted);
            manager.Arguments.Add(succededArgument);
            manager.Arguments.Add(failedArgument);
            manager.IncludeAlpha = true;
            manager.IncludeBeta = true;
            #endregion
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            rItem item = (rItem)senderView.SelectedItem;
            mp.PlayStream(item.url.ToString(), slideVol.Value);
            IsEnabled = true;
            btnStop.IsEnabled = true;
            IsPlaying = true;
        }

        private void btnStop_Click(object sender,RoutedEventArgs e)
        {
            if (IsEnabled == true) {
                if (IsPlaying == true)
                {
                    mp.Stop();
                    btnStop.IsEnabled = false;
                }
            }
        }

        public void senderListeLaden()
        {
            try
            {
                string[] array = stream.GetStreamList();
                for(int i = 0;i<array.Length - 1; i += 2)
                {
                    urlList.Add(array[i + 1]);
                    rItem item = new rItem();
                    item.Name = array[i];
                    item.url = array[i + 1];
                    senderView.Items.Add(item);
                }
            }catch(IndexOutOfRangeException e)
            {
                MessageBox.Show(String.Format("Sie haben ein Fehler: {0}", e.ToString()));
            }catch(Exception e)
            {
                MessageBox.Show(String.Format("Sie Haben ein Fehler: {0}", e.ToString()));
            }
        }

        private void slideVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mp.SetLautstaerke(slideVol.Value);
            Properties.Settings.Default.LautstrearkeWert = Convert.ToInt32(slideVol.Value);
            Properties.Settings.Default.Save();

        }

        private void mt_About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void mt_Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void mt_Updatesearch_Click(object sender, RoutedEventArgs e)
        {
            var updaterUI = new UpdaterUI(manager, SynchronizationContext.Current,true);
            updaterUI.ShowUserInterface();
        }

        private bool IsUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                return uri.Host != null;
            }
            catch
            {
                return false;
            }
        }

        private void mt_Analyzer_Click(object sender, RoutedEventArgs e)
        {
            /*Form1 form = new Form1();
            form.Show();*/
        }

        private void mt_close_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class rItem
    {
        public string Name { get; set; }
        public string url { get; set; }
    }

}
