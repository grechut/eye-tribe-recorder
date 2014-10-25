using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyeTribeRecorderUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Guid userId = Guid.NewGuid();
        private Recorder recorder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UserId.Content = userId;
            this.recorder = new Recorder(userId);
            this.MainBrowser.Navigate("http://eyespy.herokuapp.com/");
        }

        private void OpenWebsite_Click(object sender, RoutedEventArgs e)
        {
            OpenWebsite();
        }

        private void WebsiteUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OpenWebsite();
            }
        }

        private void OpenWebsite()
        {
            var url = this.WebsiteUrl.Text;
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }
            try
            {
                this.MainBrowser.Navigate(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect website address");
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                recorder.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot start recording. Reason: " + ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var res = recorder.Save();
                MessageBox.Show("Saved\n" + res);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot save");
            }
        }

        private void Zip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                recorder.Zip();
                MessageBox.Show("Zipped");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot zip");
            }
        }

    }
}
