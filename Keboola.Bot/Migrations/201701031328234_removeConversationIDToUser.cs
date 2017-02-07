using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    public class removeConversationIDToUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "ConversationID");
        }

        public override void Down()
        {
            AddColumn("dbo.Users", "ConversationID", c => c.Int(false));
        }
    }
}