namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "CreationDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Users", "CreationDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Admins", "FirstName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Admins", "LastName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Users", "DescriptionOfStudyOrWork", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "Title", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Title", c => c.String(nullable: false, maxLength: 45));
            AlterColumn("dbo.Users", "DescriptionOfStudyOrWork", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Admins", "LastName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Admins", "FirstName", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Users", "CreationDate");
            DropColumn("dbo.Admins", "CreationDate");
        }
    }
}
