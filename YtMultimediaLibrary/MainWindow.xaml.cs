using System;
using System.Collections.Generic;
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
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;

namespace YtMultimediaLibrary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var yt = new YoutubeAPIClient("AIzaSyBobUsCLkr6VrbhcwaJPWPhE5X_iuqG7j4");
            var dbContext = new DataBaseContext();

            var manager = new UserManager(yt, dbContext);
            var user = manager.Register();

            manager.AddUserChannel(user, "https://www.youtube.com/channel/UCSwtGkvmxXhWe-kK1dlm8gA", false);
            manager.AddUserChannel(user, "https://www.youtube.com/channel/UC7_tK6JLTJDYPzHR76o85vQ", false);
            
            
            var videos = yt.ChannelListLastVideos(user.Channels, 5);
            foreach (var video in videos) {
                MessageBox.Show(video.Snippet.ChannelTitle + "\n" + video.Snippet.Title);
            }
            

            /*
            foreach (var channel in user.Channels)
            {
                MessageBox.Show(channel.ChannelName);
                var videos = yt.ChannelLastVideos(channel, 5);

                foreach (var video in videos) { 
                    MessageBox.Show(video.Snippet.ChannelTitle + "\n" + video.Snippet.Title);
                }
            }
            */
            








        }
    }
}
