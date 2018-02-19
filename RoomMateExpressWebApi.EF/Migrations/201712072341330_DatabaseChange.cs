namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserReports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        IsProcessed = c.Boolean(nullable: false),
                        UserReported_Id = c.Guid(),
                        UserReporting_Id = c.Guid(),
                        Admin_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserReported_Id)
                .ForeignKey("dbo.Users", t => t.UserReporting_Id)
                .ForeignKey("dbo.Admins", t => t.Admin_Id, cascadeDelete: true)
                .Index(t => t.UserReported_Id)
                .Index(t => t.UserReporting_Id)
                .Index(t => t.Admin_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        AverageGrade = c.Decimal(nullable: false, precision: 2, scale: 1),
                        PictureUrl = c.String(nullable: false, maxLength: 255),
                        Age = c.Byte(nullable: false),
                        HasFaculty = c.Boolean(nullable: false),
                        DescriptionOfStudyOrWork = c.String(nullable: false, maxLength: 100),
                        IsSmoker = c.Boolean(nullable: false),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SentAt = c.DateTimeOffset(nullable: false, precision: 7),
                        RecievedAt = c.DateTimeOffset(precision: 7),
                        SeenAt = c.DateTimeOffset(precision: 7),
                        Text = c.String(),
                        PictureUrl = c.String(maxLength: 255),
                        UserSender_Id = c.Guid(),
                        Chat_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserSender_Id)
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .Index(t => t.UserSender_Id)
                .Index(t => t.Chat_Id);
            
            CreateTable(
                "dbo.CommentForPosts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        CommentedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        Post_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Post_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 45),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(storeType: "smallmoney"),
                        Category = c.Int(nullable: false),
                        Pets = c.Boolean(nullable: false),
                        NumberOfRoommates = c.Byte(nullable: false),
                        WantedGender = c.Int(nullable: false),
                        NumberOfViews = c.Int(nullable: false),
                        PostDate = c.DateTimeOffset(nullable: false, precision: 7),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Neighborhoods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 45),
                        City_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id, cascadeDelete: true)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostPictures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PictureUrl = c.String(nullable: false, maxLength: 255),
                        Post_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.CommentForProfiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        Grade = c.Byte(nullable: false),
                        CommentedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UserCommentator_Id = c.Guid(),
                        UserProfile_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserCommentator_Id)
                .ForeignKey("dbo.Users", t => t.UserProfile_Id)
                .Index(t => t.UserCommentator_Id)
                .Index(t => t.UserProfile_Id);
            
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Chat_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chat_Id, t.User_Id })
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Chat_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.NeighborhoodPosts",
                c => new
                    {
                        Neighborhood_Id = c.Guid(nullable: false),
                        Post_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Neighborhood_Id, t.Post_Id })
                .ForeignKey("dbo.Neighborhoods", t => t.Neighborhood_Id, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.Neighborhood_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.UserUsers",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserReports", "Admin_Id", "dbo.Admins");
            DropForeignKey("dbo.UserReports", "UserReporting_Id", "dbo.Users");
            DropForeignKey("dbo.UserReports", "UserReported_Id", "dbo.Users");
            DropForeignKey("dbo.UserUsers", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.UserUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.CommentForProfiles", "UserProfile_Id", "dbo.Users");
            DropForeignKey("dbo.CommentForProfiles", "UserCommentator_Id", "dbo.Users");
            DropForeignKey("dbo.CommentForPosts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.CommentForPosts", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Posts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PostPictures", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.NeighborhoodPosts", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.NeighborhoodPosts", "Neighborhood_Id", "dbo.Neighborhoods");
            DropForeignKey("dbo.Neighborhoods", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.ChatUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ChatUsers", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.Messages", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.Messages", "UserSender_Id", "dbo.Users");
            DropIndex("dbo.UserUsers", new[] { "User_Id1" });
            DropIndex("dbo.UserUsers", new[] { "User_Id" });
            DropIndex("dbo.NeighborhoodPosts", new[] { "Post_Id" });
            DropIndex("dbo.NeighborhoodPosts", new[] { "Neighborhood_Id" });
            DropIndex("dbo.ChatUsers", new[] { "User_Id" });
            DropIndex("dbo.ChatUsers", new[] { "Chat_Id" });
            DropIndex("dbo.CommentForProfiles", new[] { "UserProfile_Id" });
            DropIndex("dbo.CommentForProfiles", new[] { "UserCommentator_Id" });
            DropIndex("dbo.PostPictures", new[] { "Post_Id" });
            DropIndex("dbo.Neighborhoods", new[] { "City_Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.CommentForPosts", new[] { "User_Id" });
            DropIndex("dbo.CommentForPosts", new[] { "Post_Id" });
            DropIndex("dbo.Messages", new[] { "Chat_Id" });
            DropIndex("dbo.Messages", new[] { "UserSender_Id" });
            DropIndex("dbo.UserReports", new[] { "Admin_Id" });
            DropIndex("dbo.UserReports", new[] { "UserReporting_Id" });
            DropIndex("dbo.UserReports", new[] { "UserReported_Id" });
            DropTable("dbo.UserUsers");
            DropTable("dbo.NeighborhoodPosts");
            DropTable("dbo.ChatUsers");
            DropTable("dbo.CommentForProfiles");
            DropTable("dbo.PostPictures");
            DropTable("dbo.Cities");
            DropTable("dbo.Neighborhoods");
            DropTable("dbo.Posts");
            DropTable("dbo.CommentForPosts");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
            DropTable("dbo.Users");
            DropTable("dbo.UserReports");
            DropTable("dbo.Admins");
        }
    }
}
