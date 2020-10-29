namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationVerification : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VerificationCodes", "Code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VerificationCodes", "Code", c => c.String());
        }
    }
}
