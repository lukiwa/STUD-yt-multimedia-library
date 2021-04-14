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
using Google.Apis.YouTube.v3.Data;
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;
using Channel = Google.Apis.YouTube.v3.Data.Channel;

namespace YtMultimediaLibrary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            var yt = new YoutubeAPIClient("AIzaSyDueuiy9SlXlD0gMaEIlbODf345tRmaVK0");
            var dbContext = new DataBaseContext();

            var manager = new UserManager(yt, dbContext);
            var user = manager.Register();

            manager.AddUserChannel(user, "https://www.youtube.com/channel/UCSwtGkvmxXhWe-kK1dlm8gA", false);
            manager.AddUserChannel(user, "https://www.youtube.com/channel/UC7_tK6JLTJDYPzHR76o85vQ", false);
            
           
           var channelAndVideos = new Dictionary<Entities.Channel, List<SearchResult>>();
           
           foreach (var channel in user.Channels) {
                var videos = yt.ChannelLastVideos(channel, 5);
                channelAndVideos.Add(channel, videos);
           }
           ChannelsAndVideos.ItemsSource = channelAndVideos;

           /*
           For displaying as message box
           foreach (var entry in channelAndVideos) {
               MessageBox.Show(entry.Key.ChannelName);
               foreach (var video in entry.Value) {
                   MessageBox.Show(video.Snippet.ChannelTitle + "\n" + video.Snippet.Title);
               }
            }
           */
           /*
           For displaying last N videos as message box
           var videos = yt.ChannelListLastVideos(user.Channels, 5);
           foreach (var video in videos) {
               MessageBox.Show(video.Snippet.ChannelTitle + "\n" + video.Snippet.Title);
           }
           */






        }
    }
}
