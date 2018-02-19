namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAndPostChanges2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.Users", new[] { "User_Id" });
            CreateTable(
                "dbo.UserUser1",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        User_Id1 = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.User_Id1 })
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Users", t => t.User_Id1)
                .Index(t => t.User_Id)
                .Index(t => t.User_Id1);
            
            DropColumn("dbo.Users", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "User_Id", c => c.Guid());
            DropForeignKey("dbo.UserUser1", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.UserUser1", "User_Id", "dbo.Users");
            DropIndex("dbo.UserUser1", new[] { "User_Id1" });
            DropIndex("dbo.UserUser1", new[] { "User_Id" });
            DropTable("dbo.UserUser1");
            CreateIndex("dbo.Users", "User_Id");
            AddForeignKey("dbo.Users", "User_Id", "dbo.Users", "Id");
        }
    }
}
