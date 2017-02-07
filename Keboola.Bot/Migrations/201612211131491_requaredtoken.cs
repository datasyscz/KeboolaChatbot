using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    public class requaredtoken : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens");
            DropIndex("dbo.KeboolaUsers", new[] {"Token_Id"});
            AlterColumn("dbo.KeboolaUsers", "Token_Id", c => c.Int(false));
            CreateIndex("dbo.KeboolaUsers", "Token_Id");
            AddForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens", "Id", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens");
            DropIndex("dbo.KeboolaUsers", new[] {"Token_Id"});
            AlterColumn("dbo.KeboolaUsers", "Token_Id", c => c.Int());
            CreateIndex("dbo.KeboolaUsers", "Token_Id");
            AddForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens", "Id");
        }
    }
}