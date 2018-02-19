namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentModelsNameChange : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CommentForPosts", newName: "PostComments");
            RenameTable(name: "dbo.CommentForProfiles", newName: "ProfileComments");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ProfileComments", newName: "CommentForProfiles");
            RenameTable(name: "dbo.PostComments", newName: "CommentForPosts");
        }
    }
}
