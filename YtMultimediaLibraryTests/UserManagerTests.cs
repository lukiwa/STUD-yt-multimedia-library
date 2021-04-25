using NUnit.Framework;
using YtMultimediaLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Moq;
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;

namespace YtMultimediaLibrary.Tests {

    public class FakeDbSet<T> : Mock<DbSet<T>> where T : class {
        public void SetData(IEnumerable<T> data) {
            var mockDataQueryable = data.AsQueryable();

            As<IQueryable<T>>().Setup(x => x.Provider).Returns(mockDataQueryable.Provider);
            As<IQueryable<T>>().Setup(x => x.Expression).Returns(mockDataQueryable.Expression);
            As<IQueryable<T>>().Setup(x => x.ElementType).Returns(mockDataQueryable.ElementType);
            As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(mockDataQueryable.GetEnumerator());

        }
    }

    public class SetupUserManagerTests {
        protected UserManager manager = null;
        protected YoutubeAPIClient mockYt = null;
        protected FakeDbSet<User> mockUserDbSet = null;
        protected Mock<DataBaseContext> mockContext = null;
        protected FakeDbSet<Channel> mockChannelDbSet = null;

        [SetUp]
        public void Before() {
            Mock<YouTubeService> mockService = new Mock<YouTubeService>(new BaseClientService.Initializer() {
                ApiKey = "MockKey",
                ApplicationName = "YtMultimediaLibrary"
            });

            mockYt = new YoutubeAPIClient(mockService.Object);


            var users = new List<User> { new User() { UserName = "Existing", EMail = "Existing", PasswordHashed = "Existing" } };
            var channels = new List<Channel> { new Channel() { ChannelName = "ChannelName", Link = "Link", Users = users } };
            users[0].Channels = channels;

            mockUserDbSet = new FakeDbSet<User>();
            mockUserDbSet.SetData(users);

            mockChannelDbSet = new FakeDbSet<Channel>();
            mockChannelDbSet.SetData(channels);


            mockContext = new Mock<DataBaseContext>();
            mockContext.Setup(m => m.Users).Returns(mockUserDbSet.Object);

            manager = new UserManager(mockYt, mockContext.Object);
        }

        [TearDown]
        public void After() {

        }

    }

    public class UserManagerTests : SetupUserManagerTests {


        [Test()]
        public void RegisterWillCallSaveAndAddMethod() {

            manager = new UserManager(mockYt, mockContext.Object);
            manager.Register("mockUserName", "mockEmail", "mockPassword");

            mockUserDbSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

        }

        [Test()]
        public void RegisterWillThrowIfUserNameExists() {
            Assert.Throws<AuthenticationException>(() => manager.Register("Existing", "", ""));
        }

        [Test()]
        public void RegisterWillThrowIfUserEmailExists() {
            Assert.Throws<AuthenticationException>(() => manager.Register("", "Existing", ""));
        }


        [Test()]
        public void LoginWillSucceedAndReturnValidUserIfUserExists() {
            var user = manager.Login("Existing", "Existing");
            Assert.AreEqual("Existing", user.UserName);
            Assert.AreEqual("Existing", user.EMail);
            Assert.AreEqual("Existing", user.PasswordHashed);
        }

        [Test()]
        public void LoginWillThrowIfEmailNotValid() {
            Assert.Throws<AuthenticationException>(() => manager.Login("NotExisting", "Existing"));
        }

        [Test()]
        public void LoginWillThrowIfPasswordNotValid() {
            Assert.Throws<AuthenticationException>(() => manager.Login("Existing", "NotExisting"));
        }

        [Test()]
        public void GetChannelByLinkWillSucceed() {
            var channel = manager.GetChannelByLink(mockUserDbSet.Object.ToList().First(),
                "Link");

            Assert.AreEqual("ChannelName", channel.ChannelName);
            Assert.AreEqual("Link", channel.Link);
        }

        [Test()]
        public void GetChannelByDisplayNameWillSucceed() {
            var channel = manager.GetChannelByDisplayName(mockUserDbSet.Object.ToList().First(),
                "ChannelName");

            Assert.AreEqual("ChannelName", channel.ChannelName);
            Assert.AreEqual("Link", channel.Link);
            Assert.AreEqual(mockChannelDbSet.Object.ToList().First().ChannelId, channel.ChannelId);
        }



    }
}