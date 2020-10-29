namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        Path = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ChatId = c.Int(nullable: false),
                        MessageText = c.String(nullable: false, maxLength: 1024),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 70),
                        PasswordHash = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 70),
                        Bio = c.String(maxLength: 70),
                        Name = c.String(nullable: false, maxLength: 70),
                        IsActive = c.Boolean(nullable: false),
                        UserContact_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserContacts", t => t.UserContact_UserId)
                .Index(t => t.UserContact_UserId);
            
            CreateTable(
                "dbo.Avatars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserContacts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ChatUsers",
                c => new
                    {
                        Chat_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chat_Id, t.User_Id })
                .ForeignKey("dbo.Chats", t => t.Chat_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Chat_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.ChatUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ChatUsers", "Chat_Id", "dbo.Chats");
            DropForeignKey("dbo.UserContacts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserContact_UserId", "dbo.UserContacts");
            DropForeignKey("dbo.Avatars", "UserId", "dbo.Users");
            DropIndex("dbo.ChatUsers", new[] { "User_Id" });
            DropIndex("dbo.ChatUsers", new[] { "Chat_Id" });
            DropIndex("dbo.UserContacts", new[] { "UserId" });
            DropIndex("dbo.Avatars", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "UserContact_UserId" });
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Attachments", new[] { "MessageId" });
            DropTable("dbo.ChatUsers");
            DropTable("dbo.UserContacts");
            DropTable("dbo.Avatars");
            DropTable("dbo.Users");
            DropTable("dbo.Chats");
            DropTable("dbo.Messages");
            DropTable("dbo.Attachments");
        }
    }
}
