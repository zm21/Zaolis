namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIsConfirmed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.VerificationCodes", "IsConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VerificationCodes", "IsConfirmed", c => c.Boolean(nullable: false));
        }
    }
}
