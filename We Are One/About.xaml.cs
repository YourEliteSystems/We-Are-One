using System;
using System.Windows;
using System.Reflection;
using nUpdate.Updating;
namespace We_Are_One
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            txtlblUpdate();
        }

        public void txtlblUpdate()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            lblAppName.Content = "We Are One";
            lblVer.Content = version.ToString();
            lblCopy.Content = "2016-2018 (c) by Your Elite Systems, Inc.";
            lblPublisher.Content = "DarkEvolution";
        }

    }
}
