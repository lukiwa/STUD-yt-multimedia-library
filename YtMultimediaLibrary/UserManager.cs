using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Security.Authentication;
using YtMultimediaLibrary.Contexts;
using YtMultimediaLibrary.Entities;

namespace YtMultimediaLibrary {
    public class UserManager {
        private readonly YoutubeAPIClient _yt;
        private readonly DataBaseContext _dbContext;

        public UserManager(YoutubeAPIClient yt, DataBaseContext dbContext) {
            _yt = yt;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Registering new user in database
        /// </summary>
        /// <param name="userName">Nickname of the user</param>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Returns newly created user</returns>
        public User Register(string userName, string email, string password) {
            var user = _dbContext.Users.Create();
            user.UserName = userName;
            user.EMail = email;
            user.PasswordHashed = password;

            if (_dbContext.Users.Any(o => o.UserName == user.UserName || o.EMail == user.EMail))
            {
                throw new AuthenticationException("User already exists!");
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        /// <summary>
        /// Logins to account
        /// </summary>
        /// <param name="email">Email of the userr</param>
        /// <param name="password">password of the user</param>
        /// <returns>User of this session</returns>
        public User Login(string email, string password)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(user => user.EMail == email && user.PasswordHashed == password);

            if (currentUser == null)
            {
                throw new AuthenticationException("User not found in database!");
            }

            return currentUser;

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

            var currentUser = _dbContext.Users.SingleOrDefault(u => u.UserId == user.UserId);
            currentUser?.Channels.Add(channel);

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