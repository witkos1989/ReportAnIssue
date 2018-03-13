namespace ReportAnIssue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStateAndUserType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Issues", "State", c => c.Byte(nullable: false));
            AlterColumn("dbo.Issues", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Issues", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Issues", "State", c => c.String());
        }
    }
}
