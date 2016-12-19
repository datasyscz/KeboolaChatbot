namespace Keboola.Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKeboolaUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KeboolaUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeboolaTokens", t => t.Token_Id)
                .Index(t => t.Token_Id);
            
            CreateTable(
                "dbo.KeboolaTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                        KeboolaUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeboolaUsers", t => t.KeboolaUser_Id)
                .Index(t => t.KeboolaUser_Id);
            
            AddColumn("dbo.Users", "Activated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "KeboolaUser_Id", c => c.Int());
            CreateIndex("dbo.Users", "KeboolaUser_Id");
            AddForeignKey("dbo.Users", "KeboolaUser_Id", "dbo.KeboolaUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "KeboolaUser_Id", "dbo.KeboolaUsers");
            DropForeignKey("dbo.KeboolaUsers", "Token_Id", "dbo.KeboolaTokens");
            DropForeignKey("dbo.KeboolaTokens", "KeboolaUser_Id", "dbo.KeboolaUsers");
            DropIndex("dbo.KeboolaTokens", new[] { "KeboolaUser_Id" });
            DropIndex("dbo.KeboolaTokens", "KeboolaTokenIndex");
            DropIndex("dbo.KeboolaUsers", new[] { "Token_Id" });
            DropIndex("dbo.Users", new[] { "KeboolaUser_Id" });
            DropColumn("dbo.Users", "KeboolaUser_Id");
            DropColumn("dbo.Users", "Activated");
            DropTable("dbo.KeboolaTokens");
            DropTable("dbo.KeboolaUsers");
        }
    }
}
