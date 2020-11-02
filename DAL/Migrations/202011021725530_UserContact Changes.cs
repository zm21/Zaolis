namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserContactChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserContact_UserId", "dbo.UserContacts");
            DropIndex("dbo.Users", new[] { "UserContact_UserId" });
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
            
            DropColumn("dbo.Users", "UserContact_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserContact_UserId", c => c.Int());
            DropForeignKey("dbo.UserContactUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserContactUsers", "UserContact_UserId", "dbo.UserContacts");
            DropIndex("dbo.UserContactUsers", new[] { "User_Id" });
            DropIndex("dbo.UserContactUsers", new[] { "UserContact_UserId" });
            DropTable("dbo.UserContactUsers");
            CreateIndex("dbo.Users", "UserContact_UserId");
            AddForeignKey("dbo.Users", "UserContact_UserId", "dbo.UserContacts", "UserId");
        }
    }
}
