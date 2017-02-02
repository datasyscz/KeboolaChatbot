namespace Keboola.Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdsad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FrameworkId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConversationExts",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FrameworkId = c.String(),
                        Detail = c.String(),
                        BaseUri = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.UserExts", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        SendedByUser = c.Boolean(nullable: false),
                        ConversationExt_ConversationID = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.ConversationExts", t => t.ConversationExt_ConversationID)
                .Index(t => t.ConversationExt_ConversationID);
            
            CreateTable(
                "dbo.UserExts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        BaseUri = c.String(),
                        Name = c.String(),
                        BotChannel_Id = c.Int(),
                        KeboolaUser_Id = c.Int(),
                        UserChannel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.BotChannel_Id)
                .ForeignKey("dbo.KeboolaUsers", t => t.KeboolaUser_Id)
                .ForeignKey("dbo.Channels", t => t.UserChannel_Id)
                .Index(t => t.BotChannel_Id)
                .Index(t => t.KeboolaUser_Id)
                .Index(t => t.UserChannel_Id);
            
            CreateTable(
                "dbo.KeboolaUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        Token_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeboolaTokens", t => t.Token_Id, cascadeDelete: true)
                .Index(t => t.Token_Id);
            
            CreateTable(
                "dbo.KeboolaTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                        KeboolaUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeboolaUsers", t => t.KeboolaUser_Id)
                .Index(t => t.KeboolaUser_Id);
            
            CreateTable(
                "dbo.IntentAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Answer = c.String(nullable: false),
                        Advanced = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConversationExts", "User_Id", "dbo.UserExts");
            DropForeignKey("dbo.UserExts", "UserChannel_Id", "dbo.Channels");
            DropForeignKey("dbo.UserExts", "KeboolaUser_Id", "dbo.KeboolaUsers");
            DropForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens");
            DropForeignKey("dbo.KeboolaTokens", "KeboolaUser_Id", "dbo.KeboolaUsers");
            DropForeignKey("dbo.UserExts", "BotChannel_Id", "dbo.Channels");
            DropForeignKey("dbo.Messages", "ConversationExt_ConversationID", "dbo.ConversationExts");
            DropIndex("dbo.IntentAnswers", new[] { "Name" });
            DropIndex("dbo.KeboolaTokens", new[] { "KeboolaUser_Id" });
            DropIndex("dbo.KeboolaUsers", new[] { "Token_Id" });
            DropIndex("dbo.UserExts", new[] { "UserChannel_Id" });
            DropIndex("dbo.UserExts", new[] { "KeboolaUser_Id" });
            DropIndex("dbo.UserExts", new[] { "BotChannel_Id" });
            DropIndex("dbo.Messages", new[] { "ConversationExt_ConversationID" });
            DropIndex("dbo.ConversationExts", new[] { "User_Id" });
            DropTable("dbo.IntentAnswers");
            DropTable("dbo.KeboolaTokens");
            DropTable("dbo.KeboolaUsers");
            DropTable("dbo.UserExts");
            DropTable("dbo.Messages");
            DropTable("dbo.ConversationExts");
            DropTable("dbo.Channels");
        }
    }
}