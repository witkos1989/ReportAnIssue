namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPermissions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Permission_Id", "dbo.Permissions");
            DropIndex("dbo.Users", new[] { "Permission_Id" });
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Permission_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Permission_Id);
            
            DropColumn("dbo.Users", "Permission_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Permission_Id", c => c.Int());
            DropForeignKey("dbo.UserPermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users");
            DropIndex("dbo.UserPermissions", new[] { "Permission_Id" });
            DropIndex("dbo.UserPermissions", new[] { "User_Id" });
            DropTable("dbo.UserPermissions");
            CreateIndex("dbo.Users", "Permission_Id");
            AddForeignKey("dbo.Users", "Permission_Id", "dbo.Permissions", "Id");
        }
    }
}
