namespace KRNL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eleven : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Location", "Zip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Location", "Zip");
        }
    }
}
