using System;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Serialization;
using Google.Apis.YouTube.v3.Data;

namespace YtMultimediaLibrary {
    public class Video {
        public SearchResult YtApiResult { get; set; }

        public string ChannelTitle {
            get => YtApiResult.Snippet.ChannelTitle;
            private set { }
        }
        public string Title {
            get => YtApiResult.Snippet.Title;
            private set {}
        }

        public string Id
        {
            get => YtApiResult.Id.VideoId;
            private set { }

        }

        public BitmapImage Image {
            get => new BitmapImage(new Uri(YtApiResult.Snippet.Thumbnails.Default__.Url));
            private set { }
        }

        public DateTime? PublishedAt {
            get => YtApiResult.Snippet.PublishedAt;
            private set { }
        }

    }
}