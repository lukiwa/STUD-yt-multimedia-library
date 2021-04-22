using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.YouTube.v3.Data;
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;
using Channel = Google.Apis.YouTube.v3.Data.Channel;

namespace YtMultimediaLibrary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        public MainWindow()
        {
            InitializeComponent();

        }
        
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            YoutubeAPIClient yt = new YoutubeAPIClient("key");
            DataBaseContext dbContext = new DataBaseContext();
            UserManager manager = new UserManager(yt, dbContext);

            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            var user = manager.Login(email, password);

            ServiceWindow serviceWindow = new ServiceWindow(user, manager, yt, dbContext);
            Close();
            serviceWindow.Show();
        }

        //private void Register_Click(object sender, RoutedEventArgs e)
        //{
        //    string name = nameTextBox.Text;
        //    string email = newEmailTextBox.Text;
        //    string password = newPasswordTextBox.Text;
        //    string confirmedPassword = confirmPasswordTextBox.Text;


        //}


    }
}
