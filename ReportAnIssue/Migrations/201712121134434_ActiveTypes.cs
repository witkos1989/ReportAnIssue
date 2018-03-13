namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Types", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Types", "Active");
        }
    }
}
