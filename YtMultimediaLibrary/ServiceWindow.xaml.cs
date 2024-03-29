﻿using System;
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

namespace YtMultimediaLibrary {
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window {
        private User _user;
        private readonly UserManager _manager;
        private readonly YoutubeAPIClient _yt;
        private readonly DataBaseContext _dbContext;

        public ServiceWindow(User user, UserManager manager, YoutubeAPIClient yt, DataBaseContext dbContext) {
            InitializeComponent();
            _user = user;
            _manager = manager;
            _yt = yt;
            _dbContext = dbContext;
            RefreshView();
            LoginLabel.Content = "Signed as: " + _user.UserName;
        }


        private void RefreshView() {
            var channelAndVideos = new Dictionary<Entities.Channel, List<Video>>();

            foreach (var channel in _user.Channels) {
                var videos = _yt.ChannelLastVideos(channel, 5);
                channelAndVideos.Add(channel, videos);
            }
            lista.ItemsSource = channelAndVideos;
            ChannelsListView.ItemsSource = channelAndVideos.Keys;

        }

        private void VideoClickable_Click(object sender, RoutedEventArgs e) {
            var myButton = (Button)sender;
            var id = myButton.CommandParameter.ToString();
            var url = "https://www.youtube.com/watch?v=" + id;
            Process.Start(url);
        }

        private void AddChannelButton_Click(object sender, RoutedEventArgs e) {

            string url = ChannelUrlTextBox.Text;

            if (url.Length <= 0) {
                MessageBox.Show("Invalid data");
                return;
            }
            ChannelUrlTextBox.Text = "";

            _manager.AddUserChannel(_user, url, true);
            RefreshView();
        }

        private void DeleteChannelButton_Click(object sender, RoutedEventArgs e) {

            int id = int.Parse(ChannelId.Text);
            var channelToDelete = _dbContext.Channels.SingleOrDefault(x => x.ChannelId == id);
            if (channelToDelete == null) {
                MessageBox.Show("Invalid ID");
                return;
            }
            _dbContext.Channels.Remove(channelToDelete);
            _dbContext.SaveChanges();
            RefreshView();
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e) {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }



    }
}
