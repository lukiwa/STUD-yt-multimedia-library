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
using YtMultimediaLibrary.Entities;  
using System.Diagnostics;
using YtMultimediaLibrary.Contexts;

namespace YtMultimediaLibrary
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        private readonly User _user;
        private readonly UserManager _manager;
        private readonly YoutubeAPIClient _yt;
        private readonly DataBaseContext _dbContext;


        public ServiceWindow(User user, UserManager manager, YoutubeAPIClient yt, DataBaseContext dbContext)
        {
            InitializeComponent();
            _user = user;
            _manager = manager;
            _yt = yt;
            _dbContext = dbContext;
            RefreshDataGrid();


            //_manager.AddUserChannel(user, "https://www.youtube.com/channel/UCSwtGkvmxXhWe-kK1dlm8gA", false);
            //_manager.AddUserChannel(user, "https://www.youtube.com/channel/UC2bNJW1mb0VaN0-o-llWwAw", false);

            //var channelAndVideos = new Dictionary<Entities.Channel, List<Video>>();

            //foreach (var channel in _user.Channels)
            //{
            //    var videos = _yt.ChannelLastVideos(channel, 5);
            //    //ImageSource img = yt.Foo(videos.First());
            //    //imgDynamic.Source = img;
            //    channelAndVideos.Add(channel, videos);
            //}
            //lista.ItemsSource = channelAndVideos;

        }

        

        private void VideoClickable_Click(object sender, RoutedEventArgs e)
        {
            var myButton = (Button)sender;
            var id = myButton.CommandParameter.ToString();
            var url = "https://www.youtube.com/watch?v=" + id;
            Process.Start(url);
        }

        private void AddChannelButton_Click(object sender, RoutedEventArgs e)
        {
            string url = ChannelUrlTextBox.Text;
            string name = ChannelNameTextBox.Text;
                        
            var channelToAdd = new Channel
            {
                Link = url,
                ChannelName = name,
                UserId = _user.UserId
            };

            ChannelUrlTextBox.Text = "";
            ChannelNameTextBox.Text = "";

            _dbContext.Channels.Add(channelToAdd);
            _dbContext.SaveChanges();
        }

        private void DeleteChannelButton_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(ChannelId.Text);
            var channelToDelete = _dbContext.Channels.SingleOrDefault(x => x.ChannelId == id);
            _dbContext.Channels.Remove(channelToDelete);
            _dbContext.SaveChanges();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RefreshDataGrid()
        {
            //var listOfChannels = _dbContext.Channels.Where(x => x.ChannelId
            //ChannelGrid.ItemsSource = listOfChannels;
        }

    }
}
