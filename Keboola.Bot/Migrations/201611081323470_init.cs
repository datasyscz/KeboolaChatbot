using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Channels",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(),
                        FrameworkId = c.String()
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Conversations",
                    c => new
                    {
                        ConversationID = c.Int(false, true),
                        Name = c.String(),
                        FrameworkId = c.String(),
                        Detail = c.String(),
                        BaseUri = c.String(),
                        User_Id = c.Int()
                    })
                .PrimaryKey(t => t.ConversationID)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);

            CreateTable(
                    "dbo.Messages",
                    c => new
                    {
                        MessageId = c.Int(false, true),
                        Text = c.String(),
                        Date = c.DateTime(false),
                        SendedByUser = c.Boolean(false),
                        Customer_Id = c.Int(),
                        Conversation_ConversationID = c.Int()
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Users", t => t.Customer_Id)
                .ForeignKey("dbo.Conversations", t => t.Conversation_ConversationID)
                .Index(t => t.Customer_Id)
                .Index(t => t.Conversation_ConversationID);

            CreateTable(
                    "dbo.Users",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ConversationID = c.Int(false),
                        BaseUri = c.String(),
                        Name = c.String(),
                        BotChannel_Id = c.Int(),
                        UserChannel_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.BotChannel_Id)
                .ForeignKey("dbo.Channels", t => t.UserChannel_Id)
                .Index(t => t.BotChannel_Id)
                .Index(t => t.UserChannel_Id);

            CreateTable(
                    "dbo.IntentAnswers",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(maxLength: 100),
                        Answer = c.String(false),
                        Advanced = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Conversations", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "Conversation_ConversationID", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "Customer_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "UserChannel_Id", "dbo.Channels");
            DropForeignKey("dbo.Users", "BotChannel_Id", "dbo.Channels");
            DropIndex("dbo.IntentAnswers", new[] {"Name"});
            DropIndex("dbo.Users", new[] {"UserChannel_Id"});
            DropIndex("dbo.Users", new[] {"BotChannel_Id"});
            DropIndex("dbo.Messages", new[] {"Conversation_ConversationID"});
            DropIndex("dbo.Messages", new[] {"Customer_Id"});
            DropIndex("dbo.Conversations", new[] {"User_Id"});
            DropTable("dbo.IntentAnswers");
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
            DropTable("dbo.Channels");
        }
    }
}