namespace UR_HomeWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tokens", new[] { "User_Id" });
            DropColumn("dbo.Tokens", "UserId");
            RenameColumn(table: "dbo.Tokens", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Tokens", "Value", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tokens", "UserId", c => c.String(maxLength: 50));
            AlterColumn("dbo.Tokens", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User", "PassWord", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Tokens", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tokens", new[] { "UserId" });
            AlterColumn("dbo.User", "PassWord", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.Tokens", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tokens", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tokens", "Value", c => c.String(nullable: false, maxLength: 128, unicode: false));
            RenameColumn(table: "dbo.Tokens", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Tokens", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tokens", "User_Id");
        }
    }
}
