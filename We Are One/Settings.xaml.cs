using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using We_Are_One.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace We_Are_One
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            SettingsLaden();
        }

        private void SettingsLaden()
        {
            chk1.IsChecked = Properties.Settings.Default.LautStearkeSpeichern;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LautStearkeSpeichern = chk1.IsChecked.Value;
        }
		void button1_Click(object sender, RoutedEventArgs e)
		{
			
		}
    }
}
