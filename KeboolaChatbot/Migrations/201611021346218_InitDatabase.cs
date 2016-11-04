namespace KeboolaChatbot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
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
                "dbo.Conversations",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FrameworkId = c.String(),
                        Detail = c.String(),
                        BaseUri = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversationID = c.Int(nullable: false),
                        BaseUri = c.String(),
                        BotChannel_Id = c.Int(),
                        UserChannel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.BotChannel_Id)
                .ForeignKey("dbo.Channels", t => t.UserChannel_Id)
                .Index(t => t.BotChannel_Id)
                .Index(t => t.UserChannel_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        SendedByUser = c.Boolean(nullable: false),
                        Customer_Id = c.Int(),
                        Conversation_ConversationID = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Conversations", t => t.Conversation_ConversationID)
                .Index(t => t.Customer_Id)
                .Index(t => t.Conversation_ConversationID);
            
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
            DropForeignKey("dbo.Messages", "Conversation_ConversationID", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Conversations", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "UserChannel_Id", "dbo.Channels");
            DropForeignKey("dbo.Customers", "BotChannel_Id", "dbo.Channels");
            DropIndex("dbo.IntentAnswers", new[] { "Name" });
            DropIndex("dbo.Messages", new[] { "Conversation_ConversationID" });
            DropIndex("dbo.Messages", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "UserChannel_Id" });
            DropIndex("dbo.Customers", new[] { "BotChannel_Id" });
            DropIndex("dbo.Conversations", new[] { "Customer_Id" });
            DropTable("dbo.IntentAnswers");
            DropTable("dbo.Messages");
            DropTable("dbo.Customers");
            DropTable("dbo.Conversations");
            DropTable("dbo.Channels");
        }
    }
}
