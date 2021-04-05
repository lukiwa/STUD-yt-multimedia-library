using System.Collections.Generic;
using System.Linq;
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;

namespace YtMultimediaLibrary {
    public class UserManager {
        private YoutubeAPIClient _yt;
        private DataBaseContext _dbContext;

        public UserManager(YoutubeAPIClient yt, DataBaseContext dbContext) {
            _yt = yt;
            _dbContext = dbContext;
        }

        //TODO
        public User Register() {
            var user = _dbContext.Users.Create();
            user.UserName = "TestUser";
            user.EMail = "test@test.test";
            user.PasswordHashed = "PasswordHashed";
            _dbContext.Users.Add(user);

            return user;
        }

        //TODO
        public User Login() {
            return new User();
        }

        /// <summary>
        /// Add Channel to a given user (based on channel Link)
        /// </summary>
        /// <param name="user">User to which channel will be added</param>
        /// <param name="channelLink">Link to a channel</param>
        /// <param name="saveToDb">Save changes to database or not</param>
        public void AddUserChannel(User user, string channelLink, bool saveToDb = true) {
            var channel = _dbContext.Channels.Create();
            channel.Link = channelLink;
            channel.ChannelName = _yt.ChannelDisplayNameByChannelUrl(channelLink);
            _dbContext.Channels.Add(channel);

            user.Channels.Add(channel);
            if (saveToDb) {
                _dbContext.SaveChanges();
            }

        }

        /// <summary>
        /// From user channel list get channel with given display name
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channelDisplayName"></param>
        /// <returns>Given channel</returns>
        public Channel GetChannelByDisplayName(User user, string channelDisplayName) {
            return user.Channels.FirstOrDefault(channel => channel.ChannelName == channelDisplayName);
        }

        /// <summary>
        /// From user channel list get channel with link
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channelLink"></param>
        /// <returns></returns>
        public Channel GetChannelByLink(User user, string channelLink) {
            return user.Channels.FirstOrDefault(channel => channel.Link == channelLink);
        }

        /// <summary>
        /// From user channel list get channel with ID
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel GetChannelById(User user, int id) {
            return user.Channels.FirstOrDefault(channel => channel.ChannelId == id);
        }



    }
}