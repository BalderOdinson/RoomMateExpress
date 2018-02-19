namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "LastModified", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "LastModified");
        }
    }
}
