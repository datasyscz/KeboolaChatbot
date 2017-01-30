namespace Keboola.Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeConversationIDToUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "ConversationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ConversationID", c => c.Int(nullable: false));
        }
    }
}
