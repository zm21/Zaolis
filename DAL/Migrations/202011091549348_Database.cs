namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Database : DbMigration
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
                        LastMessage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.LastMessage_Id)
                .Index(t => t.LastMessage_Id);
            
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
                        LastActive = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.VerificationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RegisterVerifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserContactUsers",
                c => new
                    {
                        UserContact_UserId = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserContact_UserId, t.User_Id })
                .ForeignKey("dbo.UserContacts", t => t.UserContact_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.UserContact_UserId)
                .Index(t => t.User_Id);
            
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
            DropForeignKey("dbo.VerificationCodes", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserContacts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserContactUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserContactUsers", "UserContact_UserId", "dbo.UserContacts");
            DropForeignKey("dbo.Avatars", "UserId", "dbo.Users");
            DropForeignKey("dbo.Chats", "LastMessage_Id", "dbo.Messages");
            DropIndex("dbo.ChatUsers", new[] { "User_Id" });
            DropIndex("dbo.ChatUsers", new[] { "Chat_Id" });
            DropIndex("dbo.UserContactUsers", new[] { "User_Id" });
            DropIndex("dbo.UserContactUsers", new[] { "UserContact_UserId" });
            DropIndex("dbo.VerificationCodes", new[] { "UserId" });
            DropIndex("dbo.UserContacts", new[] { "UserId" });
            DropIndex("dbo.Avatars", new[] { "UserId" });
            DropIndex("dbo.Chats", new[] { "LastMessage_Id" });
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Attachments", new[] { "MessageId" });
            DropTable("dbo.ChatUsers");
            DropTable("dbo.UserContactUsers");
            DropTable("dbo.RegisterVerifications");
            DropTable("dbo.VerificationCodes");
            DropTable("dbo.UserContacts");
            DropTable("dbo.Avatars");
            DropTable("dbo.Users");
            DropTable("dbo.Chats");
            DropTable("dbo.Messages");
            DropTable("dbo.Attachments");
        }
    }
}
