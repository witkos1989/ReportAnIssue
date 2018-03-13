namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Accountremovecollections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserTypes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserTypes", "Type_Id", "dbo.Types");
            DropIndex("dbo.Comments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Issues", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserTypes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserTypes", new[] { "Type_Id" });
            AddColumn("dbo.AspNetUsers", "Type_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Type_Id");
            AddForeignKey("dbo.AspNetUsers", "Type_Id", "dbo.Types", "Id");
            DropColumn("dbo.Comments", "ApplicationUser_Id");
            DropColumn("dbo.Issues", "ApplicationUser_Id");
            DropTable("dbo.ApplicationUserTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserTypes",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Type_Id });
            
            AddColumn("dbo.Issues", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Comments", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "Type_Id", "dbo.Types");
            DropIndex("dbo.AspNetUsers", new[] { "Type_Id" });
            DropColumn("dbo.AspNetUsers", "Type_Id");
            CreateIndex("dbo.ApplicationUserTypes", "Type_Id");
            CreateIndex("dbo.ApplicationUserTypes", "ApplicationUser_Id");
            CreateIndex("dbo.Issues", "ApplicationUser_Id");
            CreateIndex("dbo.Comments", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserTypes", "Type_Id", "dbo.Types", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserTypes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Issues", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
