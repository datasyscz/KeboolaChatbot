using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    public partial class addKeboolauserFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeboolaUsers", "Active", c => c.Boolean(false));
            AddColumn("dbo.KeboolaTokens", "Value", c => c.String(false));
            DropColumn("dbo.Users", "Activated");
            DropColumn("dbo.KeboolaTokens", "Token");
        }

        public override void Down()
        {
            AddColumn("dbo.KeboolaTokens", "Token", c => c.String(false));
            AddColumn("dbo.Users", "Activated", c => c.Boolean(false));
            DropColumn("dbo.KeboolaTokens", "Value");
            DropColumn("dbo.KeboolaUsers", "Active");
        }
    }
}