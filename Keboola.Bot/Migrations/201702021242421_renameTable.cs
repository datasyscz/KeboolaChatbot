namespace Keboola.Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ConversationExts", newName: "Conversation");
            RenameTable(name: "dbo.UserExts", newName: "User");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.User", newName: "UserExts");
            RenameTable(name: "dbo.Conversation", newName: "ConversationExts");
        }
    }
}
