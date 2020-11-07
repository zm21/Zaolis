namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "LastMessage_Id", c => c.Int());
            CreateIndex("dbo.Chats", "LastMessage_Id");
            AddForeignKey("dbo.Chats", "LastMessage_Id", "dbo.Messages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "LastMessage_Id", "dbo.Messages");
            DropIndex("dbo.Chats", new[] { "LastMessage_Id" });
            DropColumn("dbo.Chats", "LastMessage_Id");
        }
    }
}
