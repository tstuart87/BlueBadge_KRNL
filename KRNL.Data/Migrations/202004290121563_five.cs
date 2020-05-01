namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class five : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cooperator", "IsDeleted", c => c.Int(nullable: false));
            AddColumn("dbo.Document", "IsDeleted", c => c.Int(nullable: false));
            AddColumn("dbo.Location", "IsDeleted", c => c.Int(nullable: false));
            AddColumn("dbo.Message", "IsDeleted", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "IsDeleted");
            DropColumn("dbo.Location", "IsDeleted");
            DropColumn("dbo.Document", "IsDeleted");
            DropColumn("dbo.Cooperator", "IsDeleted");
        }
    }
}
