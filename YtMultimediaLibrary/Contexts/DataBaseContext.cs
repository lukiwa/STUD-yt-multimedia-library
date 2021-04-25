using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using YtMultimediaLibrary.Entities;

namespace YtMultimediaLibrary.Contexts
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("ConnectionString") 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<User>()
                .HasMany<Channel>(s => s.Channels)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserRefId");
                    cs.MapRightKey("ChannelRefId");
                    cs.ToTable("UserChannels");
                });

        }
    }
}
