namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAndPostChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "User_Id", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "User_Id" });
            CreateTable(
                "dbo.PostUsers",
                c => new
                    {
                        Post_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Post_Id, t.User_Id })
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Post_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Users", "ProfilePictureUrl", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "BirthDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Users", "User_Id", c => c.Guid());
            AddColumn("dbo.Chats", "Name", c => c.String());
            AddColumn("dbo.Chats", "PictureUrl", c => c.String());
            AddColumn("dbo.Posts", "AccomodationOption", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "AccomodationType", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "PetOptions", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "ArePetsAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "IsSmokerAllowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Posts", "User_Id", c => c.Guid());
            CreateIndex("dbo.Users", "User_Id");
            CreateIndex("dbo.Posts", "User_Id");
            AddForeignKey("dbo.Users", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Posts", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Users", "AverageGrade");
            DropColumn("dbo.Users", "PictureUrl");
            DropColumn("dbo.Users", "Age");
            DropColumn("dbo.Posts", "Category");
            DropColumn("dbo.Posts", "Pets");
            DropColumn("dbo.Posts", "NumberOfViews");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "NumberOfViews", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "Pets", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "Category", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Age", c => c.Byte(nullable: false));
            AddColumn("dbo.Users", "PictureUrl", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "AverageGrade", c => c.Decimal(nullable: false, precision: 2, scale: 1));
            DropForeignKey("dbo.Posts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PostUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PostUsers", "Post_Id", "dbo.Posts");
            DropIndex("dbo.PostUsers", new[] { "User_Id" });
            DropIndex("dbo.PostUsers", new[] { "Post_Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            AlterColumn("dbo.Posts", "User_Id", c => c.Guid(nullable: false));
            DropColumn("dbo.Posts", "IsSmokerAllowed");
            DropColumn("dbo.Posts", "ArePetsAllowed");
            DropColumn("dbo.Posts", "PetOptions");
            DropColumn("dbo.Posts", "AccomodationType");
            DropColumn("dbo.Posts", "AccomodationOption");
            DropColumn("dbo.Chats", "PictureUrl");
            DropColumn("dbo.Chats", "Name");
            DropColumn("dbo.Users", "User_Id");
            DropColumn("dbo.Users", "BirthDate");
            DropColumn("dbo.Users", "ProfilePictureUrl");
            DropTable("dbo.PostUsers");
            CreateIndex("dbo.Posts", "User_Id");
            AddForeignKey("dbo.Posts", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
