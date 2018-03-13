namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogins : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Issues", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserPermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.TypeUsers", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.TypeUsers", "User_Id", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Issues", new[] { "UserId" });
            DropIndex("dbo.UserPermissions", new[] { "User_Id" });
            DropIndex("dbo.UserPermissions", new[] { "Permission_Id" });
            DropIndex("dbo.TypeUsers", new[] { "Type_Id" });
            DropIndex("dbo.TypeUsers", new[] { "User_Id" });
            AddColumn("dbo.AspNetUsers", "Type_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Type_Id");
            AddForeignKey("dbo.AspNetUsers", "Type_Id", "dbo.Types", "Id");
            DropTable("dbo.Permissions");
            DropTable("dbo.Users");
            DropTable("dbo.UserPermissions");
            DropTable("dbo.TypeUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TypeUsers",
                c => new
                    {
                        Type_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Type_Id, t.User_Id });
            
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Permission_Id });
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Mail = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.AspNetUsers", "Type_Id", "dbo.Types");
            DropIndex("dbo.AspNetUsers", new[] { "Type_Id" });
            DropColumn("dbo.AspNetUsers", "Type_Id");
            CreateIndex("dbo.TypeUsers", "User_Id");
            CreateIndex("dbo.TypeUsers", "Type_Id");
            CreateIndex("dbo.UserPermissions", "Permission_Id");
            CreateIndex("dbo.UserPermissions", "User_Id");
            CreateIndex("dbo.Issues", "UserId");
            CreateIndex("dbo.Comments", "UserId");
            AddForeignKey("dbo.TypeUsers", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TypeUsers", "Type_Id", "dbo.Types", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserPermissions", "Permission_Id", "dbo.Permissions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Issues", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
