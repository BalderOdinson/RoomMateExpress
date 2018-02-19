namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminAndUserChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "AccountId", c => c.Guid(nullable: false));
            AddColumn("dbo.Users", "AccountId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AccountId");
            DropColumn("dbo.Admins", "AccountId");
        }
    }
}
