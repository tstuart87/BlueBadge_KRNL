namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ninth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "LocationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "LocationCode");
        }
    }
}
