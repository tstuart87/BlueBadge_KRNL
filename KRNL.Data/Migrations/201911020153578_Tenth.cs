namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tenth : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Message", "LocationCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "LocationCode", c => c.String());
        }
    }
}
