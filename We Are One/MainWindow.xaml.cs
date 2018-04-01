#region Usings
using nUpdate.Updating;
using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.Windows;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using We_Are_One.FileExt;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;
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
        private int Time = 0;
        private Boolean IsEnabled;
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

        #region Methoden - Call_Equalizer - Frequenz-Steuerung aufrufen 
/*
        private void Call_Equalizer()
        {
            // 10-band EQ
            BASS_DX8_PARAMEQ eq = new BASS_DX8_PARAMEQ();

            _fxEQ[0] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[1] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[2] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[3] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[4] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[5] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[6] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[7] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[8] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);
            _fxEQ[9] = Bass.BASS_ChannelSetFX(_stream, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);

            eq.fBandwidth = 18f;

            // EQ1
            eq.fCenter = 80f;  // max. Tiefe
            eq.fGain = this.TrackBar1.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[0], eq);
            // EQ2
            eq.fCenter = 160f;
            eq.fGain = this.TrackBar2.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[1], eq);
            // EQ3
            eq.fCenter = 300f;
            eq.fGain = this.TrackBar3.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[2], eq);
            // EQ4
            eq.fCenter = 500f;
            eq.fGain = TrackBar4.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[3], eq);
            // EQ5
            eq.fCenter = 1000f;
            eq.fGain = TrackBar5.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[4], eq);
            // EQ6
            eq.fCenter = 3000f;
            eq.fGain = TrackBar6.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[5], eq);
            // EQ7
            eq.fCenter = 6000f;
            eq.fGain = TrackBar7.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[6], eq);
            // EQ8
            eq.fCenter = 12000f;
            eq.fGain = TrackBar8.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[7], eq);
            // EQ9
            eq.fCenter = 14000f;
            eq.fGain = TrackBar9.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[8], eq);
            // EQ10
            eq.fCenter = 16000f;
            eq.fGain = TrackBar10.Value / 10f;
            Bass.BASS_FXSetParameters(_fxEQ[9], eq);
        }


        private void UpdateEQ(int band, float gain)
        {
            BASS_DX8_PARAMEQ eq = new BASS_DX8_PARAMEQ();
            if (Bass.BASS_FXGetParameters(_fxEQ[band], eq))
            {
                eq.fGain = gain;
                Bass.BASS_FXSetParameters(_fxEQ[band], eq);
            }
        }*/
        #endregion
    }

    public class rItem
    {
        public string Name { get; set; }
        public string url { get; set; }
    }

}
