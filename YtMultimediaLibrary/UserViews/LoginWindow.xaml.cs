using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YtMultimediaLibrary.Contexts;

namespace YtMultimediaLibrary
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();


        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var yt = new YoutubeAPIClient("key");
            var dbContext = new DataBaseContext();
            var manager = new UserManager(yt, dbContext);

            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            var user = manager.Login(email, password);

            if (user == null)
                MessageBox.Show("Błędne dane");
            else
                Close();
        }
    }
}
