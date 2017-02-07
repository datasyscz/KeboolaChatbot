using System.Data.Entity.Migrations;

namespace Keboola.Bot.Migrations
{
    public class fidDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "Customer_Id", "dbo.Users");
            DropIndex("dbo.Messages", new[] {"Customer_Id"});
            DropColumn("dbo.Messages", "Customer_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Messages", "Customer_Id", c => c.Int());
            CreateIndex("dbo.Messages", "Customer_Id");
            AddForeignKey("dbo.Messages", "Customer_Id", "dbo.Users", "Id");
        }
    }
}