namespace UR_HomeWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tokens", "UserId", "dbo.User");
            DropIndex("dbo.Tokens", new[] { "UserId" });
            AlterColumn("dbo.Tokens", "Value", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Tokens", "UserId", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Tokens", "UserId");
            AddForeignKey("dbo.Tokens", "UserId", "dbo.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "UserId", "dbo.User");
            DropIndex("dbo.Tokens", new[] { "UserId" });
            AlterColumn("dbo.Tokens", "UserId", c => c.String(maxLength: 50));
            AlterColumn("dbo.Tokens", "Value", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tokens", "UserId");
            AddForeignKey("dbo.Tokens", "UserId", "dbo.User", "Id");
        }
    }
}
