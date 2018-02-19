namespace RoomMateExpressWebApi.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReportChanged2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReports", "DateReporting", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.UserReports", "DateProcessed", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.UserReports", "AdminDecision", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReports", "AdminDecision");
            DropColumn("dbo.UserReports", "DateProcessed");
            DropColumn("dbo.UserReports", "DateReporting");
        }
    }
}
