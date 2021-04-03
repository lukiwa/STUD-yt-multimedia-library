namespace YtMultimediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        ChannelId = c.Int(nullable: false, identity: true),
                        Link = c.String(nullable: false),
                        ChannelName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ChannelId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 15),
                        EMail = c.String(nullable: false, maxLength: 15),
                        PasswordHashed = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserChannels",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Channel_ChannelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Channel_ChannelId })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Channels", t => t.Channel_ChannelId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Channel_ChannelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserChannels", "Channel_ChannelId", "dbo.Channels");
            DropForeignKey("dbo.UserChannels", "User_UserId", "dbo.Users");
            DropIndex("dbo.UserChannels", new[] { "Channel_ChannelId" });
            DropIndex("dbo.UserChannels", new[] { "User_UserId" });
            DropTable("dbo.UserChannels");
            DropTable("dbo.Users");
            DropTable("dbo.Channels");
        }
    }
}
