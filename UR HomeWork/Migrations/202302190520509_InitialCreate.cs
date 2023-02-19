namespace UR_HomeWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 128, unicode: false),
                        UserId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ExpiresAt = c.DateTime(),
                        User_Id = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tokens", "User_Id", "dbo.User");
            DropIndex("dbo.Tokens", new[] { "User_Id" });
            DropTable("dbo.User");
            DropTable("dbo.Tokens");
        }
    }
}
