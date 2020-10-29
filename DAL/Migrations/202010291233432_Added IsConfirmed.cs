namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsConfirmed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VerificationCodes", "IsConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VerificationCodes", "IsConfirmed");
        }
    }
}
