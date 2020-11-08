namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SentByMe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "SentByMe", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "SentByMe");
        }
    }
}
