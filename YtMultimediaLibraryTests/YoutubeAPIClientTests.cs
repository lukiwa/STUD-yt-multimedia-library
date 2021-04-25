using NUnit.Framework;
using YtMultimediaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Moq;

namespace YtMultimediaLibrary.Tests {

    public class YoutubeAPIClientTests {

        [TestCase("https://www.youtube.com/channel/UCWTA5Yd0rAkQt5-9etIFoBA", "UCWTA5Yd0rAkQt5-9etIFoBA")]
        [TestCase("https://www.youtube.com/channel/UCxD2gCy3e0EUWN5EylcBGMA", "UCxD2gCy3e0EUWN5EylcBGMA")]
        [TestCase("https://www.youtube.com/UCxD2gCy3e0EUWN5EylcBGMA", "")]
        public void ChannelIdByChannelUrlReturnsStrippedUrl(string url, string id) {
            var result = YoutubeAPIClient.ChannelIdByChannelUrl(url);
            Assert.AreEqual(id, result);
        }

        /*
        [Test()]
        public void ChannelDisplayNameByChannelUrlWillCallYoutubeAPI() {
            var mockChannelLink = "mock";

            Mock<YouTubeService> mockService = new Mock<YouTubeService>(new BaseClientService.Initializer() {
                ApiKey = "MockKey",
                ApplicationName = "YtMultimediaLibrary"
            });

            mockService.Setup(mock => mock.Search.List("snippet"));

            YoutubeAPIClient client = new YoutubeAPIClient(mockService.Object);
            client.ChannelDisplayNameByChannelUrl(mockChannelLink);

            mockService.Verify(mock => mock.Search.List("snippet"), Times.Once());
            
        }
        */
    }
}