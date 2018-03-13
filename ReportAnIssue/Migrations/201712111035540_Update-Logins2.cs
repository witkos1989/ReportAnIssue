namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogins2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Type_Id", "dbo.Types");
            DropIndex("dbo.AspNetUsers", new[] { "Type_Id" });
            CreateTable(
                "dbo.ApplicationUserTypes",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Type_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Types", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Type_Id);
            
            AddColumn("dbo.Comments", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Issues", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            CreateIndex("dbo.Comments", "ApplicationUser_Id");
            CreateIndex("dbo.Issues", "ApplicationUser_Id");
            AddForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Issues", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "Type_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Type_Id", c => c.Int());
            DropForeignKey("dbo.ApplicationUserTypes", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.ApplicationUserTypes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserTypes", new[] { "Type_Id" });
            DropIndex("dbo.ApplicationUserTypes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Issues", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Comments", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.Issues", "ApplicationUser_Id");
            DropColumn("dbo.Comments", "ApplicationUser_Id");
            DropTable("dbo.ApplicationUserTypes");
            CreateIndex("dbo.AspNetUsers", "Type_Id");
            AddForeignKey("dbo.AspNetUsers", "Type_Id", "dbo.Types", "Id");
        }
    }
}
