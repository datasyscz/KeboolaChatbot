namespace Keboola.Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKeboolauserFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeboolaUsers", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.KeboolaTokens", "Value", c => c.String(nullable: false));
            DropColumn("dbo.Users", "Activated");
            DropColumn("dbo.KeboolaTokens", "Token");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KeboolaTokens", "Token", c => c.String(nullable: false));
            AddColumn("dbo.Users", "Activated", c => c.Boolean(nullable: false));
            DropColumn("dbo.KeboolaTokens", "Value");
            DropColumn("dbo.KeboolaUsers", "Active");
        }
    }
}
