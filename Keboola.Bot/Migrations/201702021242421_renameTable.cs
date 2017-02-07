using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    public partial class renameTable : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.ConversationExts", "Conversation");
            RenameTable("dbo.UserExts", "User");
        }

        public override void Down()
        {
            RenameTable("dbo.User", "UserExts");
            RenameTable("dbo.Conversation", "ConversationExts");
        }
    }
}