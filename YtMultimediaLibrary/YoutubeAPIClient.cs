using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Web;
using System.Windows.Documents;
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;
using ChannelEntity = YtMultimediaLibrary.Entities.Channel;

namespace YtMultimediaLibrary {
    public class YoutubeAPIClient {
        private Google.Apis.YouTube.v3.YouTubeService _service;

        public YoutubeAPIClient(string apiKey) {
            _service = new YouTubeService(new BaseClientService.Initializer() {
                ApiKey = apiKey,
                ApplicationName = this.GetType().ToString()
            });
        }

        /// <summary>
        /// Gets channel ID based on url to a channel
        /// </summary>
        /// <param name="channelUrl">url to a channel</param>
        /// <returns>channel id</returns>
        public static string ChannelIdByChannelUrl(string channelUrl) {
            var result = string.Empty;
            var channelUri = new Uri(channelUrl);
            var segments = channelUri.Segments;

            if (segments.Length > 2) {
                result = segments.Last();
            }

            return result;
        }

        /// <summary>
        /// Get Channel display name by channel Link
        /// </summary>
        /// <param name="channelLink">Link to a channel</param>
        /// <returns>Display name of a channel</returns>
        public string ChannelDisplayNameByChannelUrl(string channelLink) {
            var result = string.Empty;
            var searchListRequest = _service.Search.List("snippet");
            searchListRequest.Type = "channels";
            searchListRequest.ChannelId = ChannelIdByChannelUrl(channelLink);
            var searchListResponse = searchListRequest.Execute();

            if (searchListResponse.Items.Any()) {
                result = searchListResponse.Items[0].Snippet.ChannelTitle;
            }

            return result;
        }

        /// <summary>
        /// Retrieves list of last N videos from given channel 
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <param name="lastVideos">How many last videos should be listed</param>
        /// <returns>Last N videos</returns>
        public List<SearchResult> ChannelLastVideos(ChannelEntity channel, int lastVideos) {
            var result = new List<SearchResult>();

            var searchListRequest = _service.Search.List("snippet");
            searchListRequest.MaxResults = lastVideos;
            searchListRequest.ChannelId = ChannelIdByChannelUrl(channel.Link);
            searchListRequest.Type = "video";
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

            var searchListResponse = searchListRequest.Execute();
            result.AddRange(searchListResponse.Items);


            return result;
        }

        /// <summary>
        /// Get last N videos from the channel list - sorted by publish date
        /// </summary>
        /// <param name="channels">List of user channels</param>
        /// <param name="lastVideos">Number of last N videos</param>
        /// <returns>List of last videos</returns>
        public List<SearchResult> ChannelListLastVideos(List<ChannelEntity> channels, int lastVideos) {
            var result = new List<SearchResult>();

            foreach (var channel in channels) {
                var searchListRequest = _service.Search.List("snippet");
                searchListRequest.MaxResults = lastVideos;
                searchListRequest.ChannelId = ChannelIdByChannelUrl(channel.Link);
                searchListRequest.Type = "video";
                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

                var searchListResponse = searchListRequest.Execute();
                result.AddRange(searchListResponse.Items);
            }

            return result.OrderBy(video => video.Snippet.PublishedAt).Take(lastVideos).ToList();

        }



        /// <summary>
        /// Saves video thumbnail (photo) to a given file
        /// </summary>
        /// <param name="filepath">Full path to file where a thumbnail should be saved</param>
        /// <param name="video">Single file in form of a SearchResult</param>
        public static void SaveVideoThumbnailToFile(string filepath, SearchResult video) {
            WebClient cli = new WebClient();
            var imgBytes = cli.DownloadData(video.Snippet.Thumbnails.Default__.Url);
            File.WriteAllBytes(filepath, imgBytes);
        }
    }
}
