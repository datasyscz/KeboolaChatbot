namespace Keboola.Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requaredtoken : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens");
            DropIndex("dbo.KeboolaUsers", new[] { "Token_Id" });
            AlterColumn("dbo.KeboolaUsers", "Token_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.KeboolaUsers", "Token_Id");
            AddForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens");
            DropIndex("dbo.KeboolaUsers", new[] { "Token_Id" });
            AlterColumn("dbo.KeboolaUsers", "Token_Id", c => c.Int());
            CreateIndex("dbo.KeboolaUsers", "Token_Id");
            AddForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens", "Id");
        }
    }
}
