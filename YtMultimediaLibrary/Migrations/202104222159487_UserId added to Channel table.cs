namespace YtMultimediaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdaddedtoChanneltable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Channels", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Channels", "UserId");
        }
    }
}
