namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sixth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "MapLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "MapLink");
        }
    }
}
