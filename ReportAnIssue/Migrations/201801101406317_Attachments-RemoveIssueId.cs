namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachmentsRemoveIssueId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Files", "IssueId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "IssueId", c => c.Int(nullable: false));
        }
    }
}
