namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seven : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Location", "IsStaked", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Location", "IsStaked", c => c.Boolean(nullable: false));
        }
    }
}
