namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eleven : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cooperator", "ContactType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cooperator", "ContactType");
        }
    }
}
